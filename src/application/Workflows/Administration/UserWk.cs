using common;
using common.DTO;
using common.Exceptions;
using domain.Abstractions;
using domain.Enums;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.User;
using JsonLocalizer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace application.Workflows.Administration;

public class UserWk(
    IRepository<UserModel> repository,
    IDatabaseTransaction databaseTransaction,
    ISender<EmailDTO> sender,
    IConfiguration configuration,
    ILocalizer localizer) : Responder
{
    public async Task<Response> GetRange(PaginationDTO pagination, IEnumerable<string>? roles, bool? isBlocked)
    {
        try
        {
            var spec = new SortUserSpec(pagination.Skip, pagination.Total, pagination.ByDesc) { Roles = roles, IsBlocked = isBlocked };
            var users = await repository.GetRangeAsync(spec);
            if (users is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { users });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> GetSingle(int userId)
    {
        try
        {
            var user = await repository.GetByIdAsync(userId);
            if (user is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { user });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> BlockOrUnblock(int userId, bool block, string language = "en")
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var user = await repository.GetByIdAsync(userId);
            if (user is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            if (user.Role.Equals(Role.Admin.ToString()))
                return Response(403, localizer.Translate(Messages.FORBIDDEN));

            user.IsBlocked = block;
            await repository.UpdateAsync(user);

            var head = block ? localizer.Translate(Mail.USER_BLOCK_HEAD, language) : localizer.Translate(Mail.USER_UNBLOCK_HEAD, language);
            var body = block ? localizer.Translate(Mail.USER_BLOCK_BODY, language) : localizer.Translate(Mail.USER_UNBLOCK_BODY, language);

            await sender.SendMessage(new EmailDTO
            {
                Email = user.Email,
                Username = $"{user.FirstName} {user.LastName}",
                Subject = head,
                Body = body + configuration[App.SUPPORT_EMAIL]
            });

            transaction.Commit();
            return Response(200, new { user });
        }
        catch (Exception ex) when (ex is EntityException || ex is SmtpClientException)
        {
            transaction.Rollback();
            return Response(500, ex.Message);
        }
    }
}
