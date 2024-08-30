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
    {
        var response = await workflow.Initiate(dto);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromQuery] string token, [FromQuery] int code)
    {
        var response = await workflow.Complete(code, token);
        return StatusCode(response.Status, new { response });
    }
}
