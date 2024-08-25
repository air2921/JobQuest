using api.Startup_Extensions;
using common;
using background;
using datahub;
using Hangfire;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using infrastructure;
using application;

namespace api;

public class Startup(IWebHostEnvironment environment)
{
    private IServiceScope? _scope;

    public void ConfigureServices(IServiceCollection services)
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

        services.AddBackground();
        services.AddDataHub(config, Log.Logger);
        services.AddApplication();
        services.AddInfrastructure(config, Log.Logger);

        services.AddServices(config, environment);
    }

    private static ElasticsearchSinkOptions ConfigurationElasticSink(IConfigurationRoot configuration)
    {
        return new ElasticsearchSinkOptions(new Uri(configuration.GetConnectionString(App.ELASTIC_SEARCH)!))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"logs-{DateTime.UtcNow:yyyy}"
        };
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        _scope ??= app.ApplicationServices.CreateScope();
        var deleteExpiredAuth = _scope.ServiceProvider.GetRequiredService<DeleteExpiredAuth>();
        var deleteExpiredRecovery = _scope.ServiceProvider.GetRequiredService<DeleteExpiredRecovery>();

        app.UseHangfireDashboard();

        RecurringJob.AddOrUpdate(
            "DeleteExpiredAuthJob",
            () => deleteExpiredAuth.DeleteExpired(),
            Cron.Daily);

        RecurringJob.AddOrUpdate(
            "DeleteExpiredRecoveryJob",
            () => deleteExpiredRecovery.DeleteExpired(),
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
