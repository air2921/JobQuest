using application.Workflows.Auth;
using common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Auth;

[Route("api/registration")]
[ApiController]
public class RegistrationController(RegisterWk workflow) : ControllerBase
{
    [HttpPost("initiate")]
    public async Task<IActionResult> Initiate([FromBody] RegisterDTO dto)
        => this.Response(await workflow.Initiate(dto));

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromQuery] string token, [FromQuery] int code)
        => this.Response(await workflow.Complete(code, token));
}
