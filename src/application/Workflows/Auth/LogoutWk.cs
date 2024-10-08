﻿using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.Specifications.Auth;
using JsonLocalizer;
using System.Linq;
using System.Threading.Tasks;

namespace application.Workflows.Auth;

public class LogoutWk(
    IRepository<AuthModel> repository,
    IDatabaseTransaction databaseTransaction,
    ILocalizer localizer) : Responder
{
    public async Task<Response> Logout(string refresh, bool clearAllSessions = false)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var model = await repository.DeleteByFilterAsync(new AuthByValueSpec(refresh));
            if (model is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            if (clearAllSessions)
                await LogoutAllSessions(model.UserId);

            transaction.Commit();
            return Response(204);
        }
        catch (EntityException ex)
        {
            transaction.Rollback();
            return Response(500, ex.Message);
        }
    }

    private async Task LogoutAllSessions(int userId)
    {
        var count = repository.GetCount(new CountAuthSpec { UserId = userId });
        var modelIdentifiers = (await repository.GetRangeAsync(new SortAuthSpec(0, count, true) { UserId = userId }))
            .Select(x => x.TokenId).ToArray();

        await repository.DeleteRangeAsync(modelIdentifiers);
    }
}
