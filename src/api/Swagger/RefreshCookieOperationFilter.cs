using application.Utils;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace api.Swagger;

public class RefreshCookieOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = Immutable.REFRESH_COOKIE_KEY,
            In = ParameterLocation.Cookie,
            Description = "Cookie for automaticly updating json web token",
            Required = false
        });
    }
}
