using api.Startup_Extensions;
using application.Helpers;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace api
{
    public class Startup
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

            services.AddServices(config);
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
}
