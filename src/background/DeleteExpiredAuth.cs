using domain.Abstractions;
using domain.Models;
using domain.Specifications.Auth;
using Microsoft.Extensions.Logging;

namespace background;

public class DeleteExpiredAuth(IRepository<AuthModel> repository, ILogger<DeleteExpiredRecovery> logger)
{
    public async Task DeleteExpired()
    {
        try
        {
            var count = repository.GetCount(new CountAuthSpec { IsExpired = true });
            count = count > 10000 ? 10000 : count;

            var spec = new SortAuthSpec(0, count, false)
            {
                IsExpired = true
            };

            var tokens = await repository.GetRangeAsync(spec);
            await repository.DeleteRangeAsync(tokens.Select(token => token.TokenId).ToArray());
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex.StackTrace);
        }
    }
}
