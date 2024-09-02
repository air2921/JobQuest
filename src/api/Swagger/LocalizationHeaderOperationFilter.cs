using application.Utils;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace api.Swagger;

public class LocalizationHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = Immutable.LOCALIZATION_HEADER_NAME,
            In = ParameterLocation.Header,
            Description = "Header to define culture",
            Required = false
        });
    }
}
