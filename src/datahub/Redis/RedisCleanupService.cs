using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace datahub.Redis;

internal class RedisCleanupService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        RedisContext.Dispose();
        return Task.CompletedTask;
    }
}
