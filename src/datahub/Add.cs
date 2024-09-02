using common;
using datahub.Entity_Framework;
using datahub.Redis;
using domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace datahub;

public static class Add
{
    public static void AddDataHub(this IServiceCollection services, IHostEnvironment environment, IConfiguration config, Serilog.ILogger logger)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString(App.MAIN_DB))
            .EnableServiceProviderCaching(false)
            .EnableDetailedErrors(true)
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
        });

        using var provider = services.BuildServiceProvider();
        var dbContext = provider.GetRequiredService<AppDbContext>();
        dbContext.Initialize();

        if (environment.IsDevelopment())
            dbContext.SeedDatabase();

        services.AddLogging(log =>
        {
            log.ClearProviders();
            log.AddSerilog(logger);
        });

        services.AddSingleton(provider =>
        {
            return new RedisContext(config);
        });

        services.AddSingleton(provider =>
        {
            return new ConnectionPrimary();
        });

        services.AddSingleton(provider =>
        {
            return new ConnectionSecondary();
        });

        services.AddSingleton(provider =>
        {
            return new RedisChatConnection();
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IGenericCache<>), typeof(GenericCache<>));
        services.AddScoped<IDatabaseTransaction, DatabaseTransaction>();
        services.AddSingleton<IDataCache<ConnectionPrimary>, DataCache<ConnectionPrimary>>();
        services.AddSingleton<IDataCache<ConnectionSecondary>, DataCache<ConnectionSecondary>>();
        services.AddSingleton<IDataCache<RedisChatConnection>, DataCache<RedisChatConnection>>();

        services.AddHostedService<RedisCleanupService>();
    }
}
