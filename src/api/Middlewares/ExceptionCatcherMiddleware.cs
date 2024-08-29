using domain.Localize;
using JsonLocalizer;

namespace api.Middlewares;

internal class ExceptionCatcherMiddleware(RequestDelegate next, ILogger<ExceptionCatcherMiddleware> logger, ILocalizer localizer)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { message = localizer.Translate(Messages.ERROR_WITH_JOKE) });
            return;
        }
    }
}

public static class ExceptionCatcherMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionCatcher(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionCatcherMiddleware>();
    }
}
