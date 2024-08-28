using Microsoft.AspNetCore.Antiforgery;
using application.Utils;

namespace api.Middlewares;

internal class XsrfProtectionMiddleware(RequestDelegate next, IAntiforgery antiforgery)
{
    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey(Immutable.XSRF_HEADER_NAME))
        {
            var xsrf = context.Request.Cookies[Immutable.XSRF_COOKIE_KEY];
            if (!string.IsNullOrWhiteSpace(xsrf))
                context.Request.Headers.Append(Immutable.XSRF_HEADER_NAME, xsrf);
        }

        var requstToken = antiforgery.GetAndStoreTokens(context).RequestToken;
        if (requstToken is not null)
        {
            context.Response.Cookies.Append(
            Immutable.XSRF_COOKIE_KEY,
            requstToken,
            new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                MaxAge = TimeSpan.FromMinutes(60)
            });
        }

        await next(context);
    }
}

public static class XsrfProtectionMiddlewareExtensions
{
    public static IApplicationBuilder UseXsrfProtection(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<XsrfProtectionMiddleware>();
    }
}
