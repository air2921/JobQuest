using api.Utils;
using application.Workflows.Administration;
using domain.SpecDTO;
using JsonLocalizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Administration;

[Route("api/admin/users")]
[ApiController]
[Authorize(Policy = ApiSettings.ADMIN_POLICY)]
public class UserController(UserWk workflow, LocalizerOptions localizerOptions) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetSingle([FromRoute] int userId)
    {
        var response = await workflow.GetSingle(userId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("get/range")]
    public async Task<IActionResult> GetRange([FromQuery] int skip, [FromQuery] int total, [FromQuery] bool byDesc,
        [FromQuery] bool? isBlocked, [FromBody] IEnumerable<string>? roles)
    {
        var dto = new PaginationDTO { Skip = skip, Total = total, ByDesc = byDesc };
        var response = await workflow.GetRange(dto, roles, isBlocked);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("{userId}")]
    public async Task<IActionResult> BlockOrUnblock([FromRoute] int userId, [FromQuery] bool block,
        [FromQuery] string emailLanguage = "en")
    {
        if (!localizerOptions.SupportedLanguages.Contains(emailLanguage))
            emailLanguage = "en";

        var response = await workflow.BlockOrUnblock(userId, block, emailLanguage);
        return StatusCode(response.Status, new { response });
    }
}
