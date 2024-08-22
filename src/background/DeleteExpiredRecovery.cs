using domain.Abstractions;
using domain.Specifications.Recovery;
using domain.Models;
using Microsoft.Extensions.Logging;

namespace background;

public class DeleteExpiredRecovery(IRepository<RecoveryModel> repository, ILogger<DeleteExpiredRecovery> logger)
{
    public async Task DeleteExpired()
    {
        try
        {
            var count = repository.GetCount(new CountRecoverySpec { IsExpired = true, IsUsed = true });
            count = count > 10000 ? 10000 : count;

            var spec = new SortRecoverySpec(0, count, false)
            {
                IsUsed = true,
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
