using application.Utils;
using JsonLocalizer;
using Microsoft.Extensions.Primitives;

namespace api.Middlewares;

public class LocalizationMiddleware(RequestDelegate next, ICurrent current, LocalizerOptions localizerOptions)
{
    public Task Invoke(HttpContext context)
    {
        current.CurrentLanguage = localizerOptions.DefaultLanguage;

        if(context.Request.Headers.TryGetValue(Immutable.LOCALIZATION_HEADER_NAME, out StringValues value))
            if (localizerOptions.SupportedLanguages.Contains(value!))
                current.CurrentLanguage = value!;

        return next(context);
    }
}

public static class LocalizationMiddlewareExtensions
{
    public static IApplicationBuilder UseLocalization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LocalizationMiddleware>();
    }
}
