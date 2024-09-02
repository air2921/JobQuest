using application.Components;
using common.DTO;
using common.Exceptions;
using datahub.Redis;
using domain.Abstractions;
using domain.Enums;
using domain.Localize;
using domain.Models;
using domain.Specifications.User;
using JsonLocalizer;
using System;
using System.Threading.Tasks;

namespace application.Workflows.Auth;

public class RegisterWk(
    IRepository<UserModel> repository,
    IDatabaseTransaction databaseTransaction,
    IDataCache<ConnectionSecondary> dataCache,
    AttemptValidator attemptValidator,
    ISender<EmailDTO> sender,
    IGenerate generate,
    IHashUtility hashUtility,
    ILocalizer localizer) : Responder
{
    public async Task<Response> Initiate(RegisterDTO dto)
    {
        try
        {
            var user = await repository.GetByFilterAsync(new UserByEmailSpec(dto.Email));
            if (user is not null)
                return Response(409, localizer.Translate(Messages.USER_CONFLICT));

            var code = generate.GenerateCode(8);
            await sender.SendMessage(new EmailDTO
            {
                Email = dto.Email,
                Username = dto.AsEmployer ? localizer.Translate(Names.EMPLOYER) : localizer.Translate(Names.APPLICANT),
                Subject = localizer.Translate(Mail.REGISTRATION_CONFIRM_HEAD),
                Body = localizer.Translate(Mail.REGISTRATION_CONFIRM_BODY) + code
            });

            var uniqueToken = generate.GuidCombine(3, true);
            await dataCache.SetAsync(
                uniqueToken,
                new UserObject(dto.Email, hashUtility.Hash(dto.Password),
                dto.AsEmployer ? Role.Employer.ToString() : Role.Applicant.ToString(), code,
                dto.FirstName, dto.LastName, dto.Patronymic),
                TimeSpan.FromMinutes(10));

            return Response(200, localizer.Translate(Messages.MAIL_SENT), new { uniqueToken });
        }
        catch (Exception ex) when (ex is EntityException || ex is SmtpClientException)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> Complete(int code, string token)
    {
        using var transaction = databaseTransaction.Begin();

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

            await repository.AddAsync(new UserModel
            {
                Email = userObj.Email,
                PasswordHash = userObj.Password,
                Role = userObj.Role,
                FirstName = userObj.FirstName,
                LastName = userObj.LastName,
                Patronymic = userObj.Patronymic,
            });

            if(!await dataCache.DeleteSingleAsync(token))
                transaction.Rollback();

            transaction.Commit();
            return Response(201);
        }
        catch (EntityException ex)
        {
            transaction.Rollback();
            return Response(500, ex.Message);
        }
    }

    private record UserObject(string Email, string Password, string Role, int Code, string FirstName, string LastName, string? Patronymic);
}
