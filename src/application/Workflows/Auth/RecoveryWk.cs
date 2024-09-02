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
    IDatabaseTransaction databaseTransaction,
    ISender<EmailDTO> sender,
    IHashUtility hashUtility,
    IGenerate generate,
    ILocalizer localizer) : Responder
{
    public async Task<Response> Initiate(string email)
    {
        email = email.ToLowerInvariant();

        try
        {
            var user = await userRepository.GetByFilterAsync(new UserByEmailSpec(email));
            if (user is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            if (user.IsBlocked)
                return Response(403, localizer.Translate(Messages.FORBIDDEN));

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

            return Response(200, localizer.Translate(Messages.MAIL_SENT));
        }
        catch (Exception ex) when (ex is EntityException || ex is SmtpClientException)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> Complete(string recoveryToken, string password)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var spec = new RecoveryByValueSpec(recoveryToken) { Expressions = [x => x.User] };
            var tokenModel = await recoveryRepository.GetByFilterAsync(spec);
            if (tokenModel is null || tokenModel.User is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            if (tokenModel.User.IsBlocked)
                return Response(403, localizer.Translate(Messages.FORBIDDEN));

            tokenModel.User.PasswordHash = hashUtility.Hash(password);
            await userRepository.UpdateAsync(tokenModel.User);
            tokenModel.IsUsed = true;
            await recoveryRepository.UpdateAsync(tokenModel);

            transaction.Commit();
            return Response(204);
        }
        catch (EntityException ex)
        {
            transaction.Rollback();
            return Response(500, ex.Message);
        }
    }
}
