using application.Workflows.Auth;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Auth;

[Route("api/recovery")]
[ApiController]
public class RecoveryController(RecoveryWk workflow) : ControllerBase
{
    [HttpPost("initiate")]
    public async Task<IActionResult> Initiate([FromQuery] string email)
    {
        var response = await workflow.Initiate(email);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("complete")]
    public async Task<IActionResult> Complete([FromQuery] string token, [FromBody] string password)
    {
        var response = await workflow.Complete(token, password);
        return StatusCode(response.Status, new { response });
    }
}
