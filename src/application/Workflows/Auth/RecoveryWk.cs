using common.DTO;
using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.Specifications.Recovery;
using domain.Specifications.User;
using JsonLocalizer;
using System;
using System.Threading.Tasks;

namespace application.Workflows.Auth;

public class RecoveryWk(
    IRepository<RecoveryModel> recoveryRepository,
    IRepository<UserModel> userRepository,
    ISender<EmailDTO> sender,
    IHashUtility hashUtility,
    IGenerate generate,
    ILocalizer localizer)
{
    public async Task<Response> Initiate(string email)
    {
        email = email.ToLowerInvariant();

        try
        {
            var user = await userRepository.GetByFilterAsync(new UserByEmailSpec(email));
            if (user is null)
                return new Response { Status = 404, Message = localizer.Translate(Message.NOT_FOUND) };

            if (user.IsBlocked)
                return new Response { Status = 403, Message = localizer.Translate(Message.FORBIDDEN) };

            var uniqueToken = generate.GuidCombine(3, true);
            await sender.SendMessage(new EmailDTO
            {
                Email = email,
                Username = localizer.Translate(Names.USER),
                Subject = localizer.Translate(Mail.ACCOUNT_RECOVERY_HEAD),
                Body = localizer.Translate(Mail.ACCOUNT_RECOVERY_BODY) + uniqueToken
            });

            await recoveryRepository.AddAsync(new RecoveryModel
            {
                UserId = user.UserId,
                Value = uniqueToken,
                Expires = DateTime.UtcNow + TimeSpan.FromHours(12)
            });

            return new Response
            {
                Status = 201,
                Message = localizer.Translate(Message.MAIL_SENT),
                ObjectData = new { uniqueToken }
            };
        }
        catch (Exception ex) when (ex is EntityException || ex is SmtpClientException)
        {
            return new Response { Status = 500, Message = ex.Message };
        }
    }

    public async Task<Response> Complete(string recoveryToken, string password)
    {
        try
        {
            var spec = new RecoveryByValueSpec(recoveryToken) { Expressions = [x => x.User] };
            var tokenModel = await recoveryRepository.GetByFilterAsync(spec);
            if (tokenModel is null || tokenModel.User is null)
                return new Response { Status = 404, Message = localizer.Translate(Message.NOT_FOUND) };

            if (tokenModel.User.IsBlocked)
                return new Response { Status = 403, Message = localizer.Translate(Message.FORBIDDEN) };

            tokenModel.User.PasswordHash = hashUtility.Hash(password);
            await userRepository.UpdateAsync(tokenModel.User);

            return new Response { Status = 204 };
        }
        catch (EntityException ex)
        {
            return new Response { Status = 500, Message = ex.Message };
        }
    }
}
