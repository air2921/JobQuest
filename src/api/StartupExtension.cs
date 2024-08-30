using common;
using application.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Hangfire;
using Hangfire.PostgreSql;
using JsonLocalizer;
using api.Utils;
using domain.Enums;

namespace api;

public static class StartupExtension
{
    public static void AddStartupServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddControllers();
        services.AddSignalR();
        services.AddHttpContextAccessor();
        services.AddLogging();
        services.AddHttpClient();
        services.AddEndpointsApiExplorer();

        //services.AddResponseCompression(options =>
        //{
        //    options.EnableForHttps = true;
        //    options.Providers.Add<BrotliCompressionProvider>();
        //    options.Providers.Add<GzipCompressionProvider>();
        //});

        //services.Configure<BrotliCompressionProviderOptions>(options =>
        //{
        //    options.Level = CompressionLevel.SmallestSize;
        //});

        services.AddJsonLocalizer(env, options =>
        {
            options.LocalizationDirectory = "localization";
            options.SupportedLanguages = ["en", "ru"];
            options.DefaultLanguage = "en";
        });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddHangfire(config =>
            config.UsePostgreSqlStorage(options =>
            {
                options.UseNpgsqlConnection(configuration.GetConnectionString(App.MAIN_DB)!);
            }));

        services.AddHangfireServer();

        services.AddCors(options =>
        {
            options.AddPolicy(ApiSettings.CORS_NAME, builder =>
            {
                builder.WithOrigins(ApiSettings.FRONTEND_DOMAIN)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });

        services.AddDistributedMemoryCache();

        services.AddSession(session =>
        {
            session.IdleTimeout = TimeSpan.FromMinutes(15);
            session.Cookie.HttpOnly = true;
            session.Cookie.SameSite = SameSiteMode.None;
            session.Cookie.IsEssential = true;
            session.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        services.AddAuthorizationBuilder()
            .AddPolicy(ApiSettings.ADMIN_POLICY, policy =>
            {
                policy.RequireRole(Role.Admin.ToString());
            })
            .AddPolicy(ApiSettings.EMPLOYER_POLICY, policy =>
            {
                policy.RequireRole(Role.Employer.ToString(), Role.Admin.ToString());
            })
            .AddPolicy(ApiSettings.APPLICANT_POLICY, policy =>
            {
                policy.RequireRole(Role.Applicant.ToString(), Role.Admin.ToString());
            });

        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwt =>
        {
            jwt.RequireHttpsMetadata = true;
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration[App.SECRET_KEY]!)),
                ValidIssuer = configuration[App.ISSUER],
                ValidAudience = configuration[App.AUDIENCE],
                ClockSkew = TimeSpan.Zero
            };
            jwt.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = auth =>
                {
                    auth.Response.StatusCode = 401;
                    auth.Response.ContentType = "application/json";
                    if (auth.Request.Cookies.ContainsKey(Immutable.REFRESH_COOKIE_KEY))
                        auth.Response.Headers.Append("X-AUTH-REQUIRED", true.ToString());

                    return auth.Response.WriteAsJsonAsync(new { message = "Invalid auth token" });
                }
            };
        });

        services.AddScoped<IUserInfo, UserInfo>();
    }
}
