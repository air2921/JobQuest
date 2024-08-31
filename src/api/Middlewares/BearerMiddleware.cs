using application.Utils;
using application.Components;
using common.Exceptions;

namespace api.Middlewares;

internal class BearerMiddleware(RequestDelegate next, ILogger<BearerMiddleware> logger, IWebHostEnvironment env)
{
    public async Task Invoke(HttpContext context, SessionComponent component)
    {
        try
        {
            AddSecurityHeaders(context);

            if (context.Request.Headers.ContainsKey(Immutable.NONE_BEARER))
            {
                await next(context);
                return;
            }

            context.Request.Cookies.TryGetValue(Immutable.JWT_COOKIE_KEY, out string? requestJwt);
            if (!string.IsNullOrWhiteSpace(requestJwt))
            {
                context.Request.Headers.Append("Authorization", $"Bearer {requestJwt}");

                await next(context);
                return;
            }

            context.Request.Cookies.TryGetValue(Immutable.REFRESH_COOKIE_KEY, out string? requestRefresh);
            if (requestRefresh is not null)
            {
                if (env.IsDevelopment())
                    logger.LogWarning(requestRefresh);

                var response = await component.RefreshJsonWebToken(requestRefresh);
                if (!response.IsSuccess || response.ObjectData is not string jwt)
                {
                    if (!response.IsSuccess)
                        context.Response.Cookies.Delete(Immutable.REFRESH_COOKIE_KEY);

                    await next(context);
                    return;
                }

                context.Response.Cookies.Append(Immutable.JWT_COOKIE_KEY, jwt, new CookieOptions
                {
                    MaxAge = Immutable.JwtExpires,
                    Secure = true,
                    HttpOnly = false,
                    SameSite = SameSiteMode.None,
                });
                context.Request.Headers.Append("Authorization", $"Bearer {jwt}");
            }

            await next(context);
            return;
        }
        catch (EntityException ex)
        {
            logger.LogError(ex.ToString());

            await next(context);
            return;
        }
    }

    private static void AddSecurityHeaders(HttpContext context)
    {
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Xss-Protection", "1");
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self' https://cdnjs.cloudflare.com; style-src 'self' https://fonts.googleapis.com; font-src 'self' https://fonts.gstatic.com; img-src 'self' data:;");
    }
}

public static class BearerMiddlewareExtensions
{
    public static IApplicationBuilder UseBearer(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BearerMiddleware>();
    }
}
