using application.Components;
using application.Utils;
using application.Workflows.Administration;
using application.Workflows.Auth;
using application.Workflows.Chat;
using application.Workflows.Core;
using application.Workflows.Core.Favorites;
using AutoMapper;
using common.DTO;
using domain.Abstractions;
using domain.Models;
using JsonLocalizer;
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

        services.AddScoped(provider =>
        {
            var repository = provider.GetRequiredService<IRepository<UserModel>>();
            var transaction = provider.GetRequiredService<IDatabaseTransaction>();
            var sender = provider.GetRequiredService<ISender<EmailDTO>>();
            var localizer = provider.GetRequiredService<ILocalizer>();
            return new UserWk(repository, transaction, sender, configuration, localizer);
        });

        services.AddAutoMapper(typeof(application.AutoMapper.Mapper));
         
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
        services.AddScoped<ResumeWk>();
        services.AddScoped<VacancyWk>();

        services.AddScoped<ChatWk>();
        services.AddScoped<MessageWk>();
    }
}
