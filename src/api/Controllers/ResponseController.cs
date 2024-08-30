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
    {
        var response = await workflow.GetRangeAsEmployer(dto, vacancyId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("get/as/applicant")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> GetResponsesAsApplicant([FromBody] SortResponseDTO dto, [FromQuery] int resumeId)
    {
        var response = await workflow.GetRangeAsApplicant(dto, resumeId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpGet("{responseId}")]
    [Authorize]
    public async Task<IActionResult> GetResponse([FromRoute] int responseId)
    {
        var response = await workflow.GetSingle(responseId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete("{responseId}")]
    [Authorize]
    public async Task<IActionResult> DeleteResponse([FromRoute] int responseId)
    {
        var response = await workflow.RemoveSingle(responseId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("add/{resumeId}/{vacancyId}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddResponse([FromRoute] int resumeId, [FromRoute] int vacancyId)
    {
        var response = await workflow.AddSingle(resumeId, vacancyId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPut("{responseId}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> UpdateResponse([FromBody] ResponseDTO dto, [FromRoute] int responseId)
    {
        var response = await workflow.Update(dto, responseId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }
}
