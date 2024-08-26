using application.Utils;
using domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace application;

public static class Add
{
    public static void AddApplication(this IServiceCollection services,
        IConfiguration configuration, Serilog.ILogger logger)
    {
        services.AddLogging(log =>
        {
            log.ClearProviders();
            log.AddSerilog(logger);
        });

        services.AddScoped(provider =>
        {
            var generator = provider.GetRequiredService<IGenerate>();
            return new TokenPublisher(configuration, generator);
        });
    }
}
