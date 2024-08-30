using application.Components;
using application.Utils;
using common.DTO;
using common.Exceptions;
using datahub.Redis;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.Specifications.Company;
using domain.Specifications.User;
using JsonLocalizer;
using System;
using System.Threading.Tasks;

namespace application.Workflows.Auth;

public class LoginWk(
    IRepository<UserModel> userRepository,
    IRepository<AuthModel> authRepository,
    IRepository<CompanyModel> companyRepository,
    ISender<EmailDTO> sender,
    IHashUtility hashUtility,
    IGenerate generate,
    IDataCache<ConnectionPrimary> dataCache,
    AttemptValidator attemptValidator,
    TokenPublisher tokenPublisher,
    ILocalizer localizer) : Responder
{
    public async Task<Response> Initiate(LoginDTO dto)
    {
        try
        {
            if (!await attemptValidator.IsValidTry(dto.Email))
                return Response(403, localizer.Translate(Messages.TOO_MANY_ATTEMPTS));

            var user = await userRepository.GetByFilterAsync(new UserByEmailSpec(dto.Email));
            if (user is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            if (user.IsBlocked)
                return Response(403, localizer.Translate(Messages.FORBIDDEN));

            if (!hashUtility.Verify(dto.Password, user.PasswordHash))
            {
                await attemptValidator.AddAttempt(dto.Email);
                return Response(401, localizer.Translate(Messages.INCORRECT_LOGIN));
            }

            var code = generate.GenerateCode(8);
            await sender.SendMessage(new EmailDTO
            {
                Email = dto.Email,
                Username = localizer.Translate(Names.USER),
                Subject = localizer.Translate(Mail.LOGIN_CONFIRM_HEAD),
                Body = localizer.Translate(Mail.LOGIN_CONFIRM_BODY) + code
            });

            var uniqueToken = generate.GuidCombine(3, true);
            await dataCache.SetAsync(uniqueToken, new UserObject(code, user.UserId, user.Role), TimeSpan.FromMinutes(10));

            return Response(200, localizer.Translate(Messages.MAIL_SENT), new { uniqueToken });
        }
        catch (Exception ex) when (ex is EntityException || ex is SmtpClientException)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> Complete(int code, string token)
    {
        try
        {
            if (!await attemptValidator.IsValidTry(token))
                return Response(403, localizer.Translate(Messages.TOO_MANY_ATTEMPTS));

            var userObj = await dataCache.GetSingleAsync<UserObject>(token);
            if (userObj is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            if (!code.Equals(userObj.Code))
            {
                await attemptValidator.AddAttempt(token);
                return Response(403, localizer.Translate(Messages.INCORRECT_CODE));
            }

            var now = DateTime.UtcNow;
            var refresh = tokenPublisher.RefreshToken();
            await authRepository.AddAsync(new AuthModel
            {
                CreatedAt = now,
                Expires = now + Immutable.RefreshExpires,
                UserId = userObj.UserId,
                Value = refresh
            });

            var company = await companyRepository.GetByFilterAsync(new CompanyByRelationSpec(userObj.UserId));
            int? companyId = company?.CompanyId;

            return Response(200, new
            {
                refresh,
                auth = tokenPublisher.JsonWebToken(new JwtDTO
                {
                    Expires = Immutable.JwtExpires,
                    Role = userObj.Role,
                    CompanyId = companyId,
                    UserId = userObj.UserId
                })
            });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    private record UserObject(int Code, int UserId, string Role);
}
