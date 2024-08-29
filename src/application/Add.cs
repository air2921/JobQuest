using application.Components;
using application.Utils;
using application.Workflows.Auth;
using AutoMapper;
using domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddScoped<LoginWk>();
        services.AddScoped<LogoutWk>();
        services.AddScoped<RegisterWk>();
        services.AddScoped<RecoveryWk>();
    }
}
