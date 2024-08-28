using application.Components;
using application.Utils;
using common.DTO;
using common.Exceptions;
using datahub.Redis;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.Specifications.User;
using JsonLocalizer;
using System;
using System.Threading.Tasks;

namespace application.Workflows.Auth;

public class LoginWk(
    IRepository<UserModel> userRepository,
    IRepository<AuthModel> authRepository,
    ISender<EmailDTO> sender,
    IHashUtility hashUtility,
    IGenerate generate,
    IDataCache<ConnectionPrimary> dataCache,
    AttemptValidator attemptValidator,
    TokenPublisher tokenPublisher,
    ILocalizer localizer)
{
    public async Task<Response> Initiate(string email, string password)
    {
        email = email.ToLowerInvariant();

        try
        {
            if (!await attemptValidator.IsValidTry(email))
                return new Response { Status = 403, Message = localizer.Translate(Message.TOO_MANY_ATTEMPTS) };

            var user = await userRepository.GetByFilterAsync(new UserByEmailSpec(email));
            if (user is null)
                return new Response { Status = 404, Message = localizer.Translate(Message.NOT_FOUND) };

            if (user.IsBlocked)
                return new Response { Status = 403, Message = localizer.Translate(Message.FORBIDDEN) };

            if (!hashUtility.Verify(password, user.PasswordHash))
            {
                await attemptValidator.AddAttempt(email);
                return new Response { Status = 403, Message = localizer.Translate(Message.INCORRECT_LOGIN) };
            }

            var code = generate.GenerateCode(8);
            await sender.SendMessage(new EmailDTO
            {
                Email = email,
                Username = localizer.Translate(Names.USER),
                Subject = localizer.Translate(Mail.LOGIN_CONFIRM_HEAD),
                Body = localizer.Translate(Mail.LOGIN_CONFIRM_BODY) + code
            });

            var uniqueToken = generate.GuidCombine(3, true);
            await dataCache.SetAsync(uniqueToken, new UserObject(code, user.UserId, user.Role), TimeSpan.FromMinutes(10));

            return new Response
            {
                Status = 200,
                Message = localizer.Translate(Message.MAIL_SENT),
                ObjectData = new { uniqueToken }
            };
        }
        catch (Exception ex) when (ex is EntityException || ex is SmtpClientException)
        {
            return new Response { Status = 500, Message = ex.Message };
        }
    }

    public async Task<Response> Complete(int code, string token)
    {
        try
        {
            if (!await attemptValidator.IsValidTry(token))
                return new Response { Status = 403, Message = localizer.Translate(Message.TOO_MANY_ATTEMPTS) };

            var userObj = await dataCache.GetSingleAsync<UserObject>(token);
            if (userObj is null)
                return new Response { Status = 404, Message = localizer.Translate(Message.NOT_FOUND) };

            if (!code.Equals(userObj.Code))
            {
                await attemptValidator.AddAttempt(token);
                return new Response { Status = 403, Message = localizer.Translate(Message.INCORRECT_CODE) };
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

            return new Response
            {
                Status = 200,
                ObjectData = new 
                { 
                    refresh,
                    auth = tokenPublisher.JsonWebToken(new JwtDTO
                    {
                        Expires = Immutable.JwtExpires,
                        Role = userObj.Role,
                        UserId = userObj.UserId
                    })
                }
            };
        }
        catch (EntityException ex)
        {
            return new Response { Status = 500, Message = ex.Message };
        }
    }

    private record UserObject(int Code, int UserId, string Role);
}
