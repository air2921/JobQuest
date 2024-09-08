using application.Workflows;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

internal static class ApiResponder
{
    internal static IActionResult Response(this ControllerBase controller, Response response)
    {
        ArgumentNullException.ThrowIfNull(response);
        return controller.StatusCode(response.Status, new { response });
    }
}
