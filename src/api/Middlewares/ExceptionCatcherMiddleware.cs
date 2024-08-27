namespace api.Middlewares;

internal class ExceptionCatcherMiddleware(RequestDelegate next, ILogger<ExceptionCatcherMiddleware> logger)
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
            await context.Response.WriteAsJsonAsync(new { message = "Unexpected error. Don't worry, we already working on it" });
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
