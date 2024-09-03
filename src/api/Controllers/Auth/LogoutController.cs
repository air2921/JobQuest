using application.Utils;
using application.Workflows.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Auth;

[Route("api/logout")]
[ApiController]
public class LogoutController(LogoutWk workflow) : ControllerBase
{
    [HttpPost("cookie")]
    [Authorize]
    public async Task<IActionResult> LogoutFromCookie([FromQuery] bool clearAll = false)
    {
        if (!HttpContext.Request.Cookies.TryGetValue(Immutable.REFRESH_COOKIE_KEY, out string? refresh))
            return StatusCode(401);

        var response = await workflow.Logout(refresh, clearAll);
        return StatusCode(response.Status);
    }

    [HttpPost("query")]
    [Authorize]
    public async Task<IActionResult> LogoutFromQuery([FromQuery] string refresh, [FromQuery] bool clearAll = false)
    {
        var response = await workflow.Logout(refresh, clearAll);
        return StatusCode(response.Status);
    }
}
