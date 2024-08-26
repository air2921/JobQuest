﻿using common;
using background;
using datahub;
using Hangfire;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using infrastructure;
using application;

namespace api;

internal class Startup(IWebHostEnvironment environment)
{
    internal void ConfigureServices(IServiceCollection services)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Startup>()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables()
            .Build();

        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .Enrich.FromLogContext()
           .Enrich.WithProperty("Environment", env)
           .ReadFrom.Configuration(config)
           .WriteTo.Console()
           .WriteTo.Elasticsearch(ConfigurationElasticSink(config))
           .CreateLogger();

        services.AddBackground(Log.Logger);
        services.AddDataHub(config, Log.Logger);
        services.AddApplication(config, Log.Logger);
        services.AddInfrastructure(config, Log.Logger);
        services.AddStartupServices(config, environment);
    }

    private static ElasticsearchSinkOptions ConfigurationElasticSink(IConfigurationRoot configuration)
    {
        return new ElasticsearchSinkOptions(new Uri(configuration.GetConnectionString(App.ELASTIC_SEARCH)!))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"logs-{DateTime.UtcNow:yyyy}"
        };
    }

    internal void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<DeleteExpiredAuth>();
        var recoveryService = scope.ServiceProvider.GetRequiredService<DeleteExpiredRecovery>();

        app.UseHangfireDashboard();

        RecurringJob.AddOrUpdate(
            "DeleteExpiredAuthJob",
            () => authService.DeleteExpired(),
            Cron.Daily);

        RecurringJob.AddOrUpdate(
            "DeleteExpiredRecoveryJob",
            () => recoveryService.DeleteExpired(),
            Cron.Daily);

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (env.IsProduction())
            app.UseHsts();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseSession();
        app.UseCors("AllowSpecificOrigin");
        //app.UseBearer();
        app.UseAuthentication();
        app.UseAuthorization();
        //app.UseXSRF();
        //app.UseExceptionHandle();

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllers();
        });
    }
}
