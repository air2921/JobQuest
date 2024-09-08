using application.Workflows.Auth;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Auth;

[Route("api/recovery")]
[ApiController]
public class RecoveryController(RecoveryWk workflow) : ControllerBase
{
    [HttpPost("initiate")]
    public async Task<IActionResult> Initiate([FromQuery] string email)
        => this.Response(await workflow.Initiate(email));

    [HttpPost("complete")]
    public async Task<IActionResult> Complete([FromQuery] string token, [FromBody] string password)
        => this.Response(await workflow.Complete(token, password));
}
