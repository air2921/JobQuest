using application.Components;
using application.Utils;
using application.Workflows.Auth;
using application.Workflows.Chat;
using application.Workflows.Core;
using application.Workflows.Core.Favorites;
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

        services.AddScoped<FavoritesResumeWk>();
        services.AddScoped<FavoritesVacancyWk>();
        services.AddScoped<CompanyWk>();
        services.AddScoped<EducationWk>();
        services.AddScoped<ExperienceWk>();
        services.AddScoped<LanguageWk>();
        services.AddScoped<ResponseWk>();
        services.AddScoped<ReviewWk>();
        services.AddScoped<ResumeWK>();
        services.AddScoped<VacancyWk>();

        services.AddScoped<ChatWk>();
        services.AddScoped<MessageWk>();
    }
}
