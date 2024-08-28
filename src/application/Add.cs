using application.Components;
using application.Utils;
using AutoMapper;
using domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace application;

public static class Add
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(provider =>
        {
            var generator = provider.GetRequiredService<IGenerate>();
            return new TokenPublisher(configuration, generator);
        });

        services.AddAutoMapper(typeof(Mapper));

        services.AddScoped<SessionComponent>();
        services.AddScoped<AttemptValidator>();
    }
}
