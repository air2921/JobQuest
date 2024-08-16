using common.DTO;
using domain.Abstractions;
using infrastructure.Abstractions;
using infrastructure.EmailSender;
using infrastructure.S3;
using infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace infrastructure;

public static class Add
{
    public static void AddInfrastructure(this IServiceCollection services,
        IConfiguration config, Serilog.ILogger logger)
    {
        services.AddLogging(log =>
        {
            log.ClearProviders();
            log.AddSerilog(logger);
        });

        services.AddScoped<IS3ClientProvider>(provider =>
        {
            return new S3ClientProvider(config);
        });

        services.AddScoped<ISmtpClientWrapper>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<SmtpClientWrapper>>();
            return new SmtpClientWrapper(logger, config);
        });

        services.AddScoped<ISender<EmailDTO>>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<Sender>>();
            var smtpClient = provider.GetRequiredService<ISmtpClientWrapper>();
            return new Sender(config, logger, smtpClient);
        });

        services.AddScoped<IS3Service, S3Service>();
        services.AddScoped<IGenerate, Generate>();
        services.AddScoped<IHashUtility, HashUtility>();

    }
}
