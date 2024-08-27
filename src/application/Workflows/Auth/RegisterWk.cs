using common.DTO;
using common.Exceptions;
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
    IDataCache dataCache,
    ISender<EmailDTO> sender,
    IGenerate generate,
    IHashUtility hashUtility,
    ILocalizer localizer)
{
    public async Task<Response> Initiate(RegisterDTO dto)
    {
        try
        {
            var user = await repository.GetByFilterAsync(new UserByEmailSpec(dto.Email));
            if (user is not null)
                return new Response { Status = 409, Message = localizer.Translate(Message.USER_CONFLICT) };

            var code = generate.GenerateCode(8);
            await sender.SendMessage(new EmailDTO
            {
                Email = dto.Email,
                Username = dto.AsEmployer ? localizer.Translate(Names.EMPLOYER) : localizer.Translate(Names.APPLICANT),
                Subject = localizer.Translate(Mail.REGISTRATION_CONFIRM_HEAD),
                Body = localizer.Translate(Mail.REGISTRATION_CONFIRM_BODY) + code
            });

            var uniqueToken = generate.GuidCombine(3);
            await dataCache.SetAsync(
                uniqueToken,
                new UserObject(dto.Email, hashUtility.Hash(dto.Password),
                dto.AsEmployer ? Role.Employer.ToString() : Role.Applicant.ToString(), code),
                TimeSpan.FromMinutes(10));

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
            var userObj = await dataCache.GetSingleAsync<UserObject>(token);
            if (userObj is null)
                return new Response { Status = 404, Message = localizer.Translate(Message.NOT_FOUND) };

            if (!code.Equals(userObj.Code))
                return new Response { Status = 403, Message = localizer.Translate(Message.INCORRECT_CODE) };

            await repository.AddAsync(new UserModel
            {
                Email = userObj.Email,
                PasswordHash = userObj.Password,
                Role = userObj.Role,
            });

            return new Response { Status = 201 };
        }
        catch (EntityException ex)
        {
            return new Response { Status = 500, Message = ex.Message };
        }
    }

    private record UserObject(string Email, string Password, string Role, int Code);
}
