using common.DTO;
using domain.Abstractions;
using infrastructure.Abstractions;
using infrastructure.EmailSender;
using infrastructure.S3;
using infrastructure.Utils;
using JsonLocalizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace infrastructure;

public static class Add
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped(provider =>
        {
            return new S3ClientProvider(config);
        });

        services.AddScoped<ISmtpClientWrapper>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<SmtpClientWrapper>>();
            var localizer = provider.GetRequiredService<ILocalizer>();
            return new SmtpClientWrapper(logger, config, localizer);
        });

        services.AddScoped<ISender<EmailDTO>>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<Sender>>();
            var smtpClient = provider.GetRequiredService<ISmtpClientWrapper>();
            var localizer = provider.GetRequiredService<ILocalizer>();
            return new Sender(config, logger, smtpClient, localizer);
        });

        services.AddScoped<IS3Service, S3Service>();
        services.AddScoped<IGenerate, Generate>();
        services.AddScoped<IHashUtility, HashUtility>();
    }
}
