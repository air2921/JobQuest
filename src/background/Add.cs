using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace background;

public static class Add
{
    public static void AddBackground(this IServiceCollection services)
    {
        services.AddScoped<DeleteExpiredAuth>();
        services.AddScoped<DeleteExpiredRecovery>();
    }
}
