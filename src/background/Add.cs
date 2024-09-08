using Microsoft.Extensions.DependencyInjection;

namespace background;

public static class Add
{
    public static void AddBackground(this IServiceCollection services)
    {
        services.AddScoped<DeleteExpiredAuth>();
        services.AddScoped<DeleteExpiredRecovery>();

        services.AddHostedService<RedisCleanupService>();
        services.AddHostedService<JsonLocInitializer>();
    }
}
