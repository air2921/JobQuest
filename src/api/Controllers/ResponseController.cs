using api.Utils;
using application.Workflows.Core;
using common.DTO.ModelDTO;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/responses")]
[ApiController]
public class ResponseController(ResponseWk workflow, IUserInfo userInfo) : ControllerBase
{
    [HttpPost("get/as/employer")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> GetResponsesAsEmployer([FromBody] SortResponseDTO dto, [FromQuery] int vacancyId)
        => this.Response(await workflow.GetRangeAsEmployer(dto, vacancyId, userInfo.UserId));

    [HttpPost("get/as/applicant")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> GetResponsesAsApplicant([FromBody] SortResponseDTO dto, [FromQuery] int resumeId)
        => this.Response(await workflow.GetRangeAsApplicant(dto, resumeId, userInfo.UserId));

    [HttpGet("{responseId}")]
    [Authorize]
    public async Task<IActionResult> GetResponse([FromRoute] int responseId)
        => this.Response(await workflow.GetSingle(responseId, userInfo.UserId));

    [HttpDelete("{responseId}")]
    [Authorize]
    public async Task<IActionResult> DeleteResponse([FromRoute] int responseId)
        => this.Response(await workflow.RemoveSingle(responseId, userInfo.UserId));

    [HttpPost("add/{resumeId}/{vacancyId}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddResponse([FromRoute] int resumeId, [FromRoute] int vacancyId)
        => this.Response(await workflow.AddSingle(resumeId, vacancyId, userInfo.UserId));

    [HttpPut("{responseId}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> UpdateResponse([FromBody] ResponseDTO dto, [FromRoute] int responseId)
        => this.Response(await workflow.Update(dto, responseId, userInfo.UserId));
}
