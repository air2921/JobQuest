using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace background;

public static class Add
{
    public static void AddBackground(this IServiceCollection services, Serilog.ILogger logger)
    {
        services.AddLogging(log =>
        {
            log.ClearProviders();
            log.AddSerilog(logger);
        });

        services.AddScoped<DeleteExpiredAuth>();
        services.AddScoped<DeleteExpiredRecovery>();
    }
}
