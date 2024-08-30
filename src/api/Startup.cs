using common;
using background;
using datahub;
using Hangfire;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using infrastructure;
using application;
using api.Middlewares;
using api.Hubs;
using api.Utils;

namespace api;

public class Startup(IWebHostEnvironment environment)
{
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

        services.AddLogging(log =>
        {
            log.ClearProviders();
            log.AddSerilog(Log.Logger);
        });

        services.AddBackground();
        services.AddDataHub(config);
        services.AddApplication(config);
        services.AddInfrastructure(config);
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

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //app.UseResponseCompression();
        app.UseHangfireDashboard("/hangfire");

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
        app.UseCors(ApiSettings.CORS_NAME);
        app.UseBearer();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseExceptionCatcher();

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllers();
            endpoint.MapHub<ChatHub>("/chats");
        });
    }
}
