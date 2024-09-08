using datahub.Redis;
using Microsoft.Extensions.Hosting;

namespace background;

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
