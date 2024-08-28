﻿using common;
using datahub.Entity_Framework;
using datahub.Redis;
using domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace datahub;

public static class Add
{
    public static void AddDataHub(this IServiceCollection services,
        IConfiguration config, Serilog.ILogger logger)
    {
        services.AddLogging(log =>
        {
            log.ClearProviders();
            log.AddSerilog(logger);
        });

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString(App.MAIN_DB))
            .EnableServiceProviderCaching(false)
            .EnableDetailedErrors(true)
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
        });

        services.AddSingleton(provider =>
        {
            return new RedisContext(config);
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IDatabaseTransaction, DatabaseTransaction>();
        services.AddSingleton<IDataCache<ConnectionPrimary>, DataCache<ConnectionPrimary>>();
        services.AddSingleton<IDataCache<ConnectionSecondary>, DataCache<ConnectionSecondary>>();

        services.AddHostedService<RedisCleanupService>();
    }
}
