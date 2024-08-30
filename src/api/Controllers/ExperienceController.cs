using api.Utils;
using application.Workflows.Core;
using common.DTO.ModelDTO;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/experiences")]
[ApiController]
public class ExperienceController(ExperienceWk workflow) : ControllerBase
{
    [HttpPost("add/{resumeId}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddEducation([FromBody] ExperienceDTO dto, [FromRoute] int resumeId, IUserInfo userInfo)
    {
        var response = await workflow.AddSingle(dto, resumeId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("add/range/{resumeId}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddEducations([FromBody] IEnumerable<ExperienceDTO> dtos, [FromRoute] int resumeId, IUserInfo userInfo)
    {
        var response = await workflow.AddRange(dtos, resumeId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> DeleteEducation([FromRoute] int id, IUserInfo userInfo)
    {
        var response = await workflow.RemoveSingle(id, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEducation([FromRoute] int id)
    {
        var response = await workflow.GetSingle(id);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("get")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEducations([FromBody] SortExperienceDTO dto, [FromQuery] int companyId)
    {
        var response = await workflow.GetRange(dto);
        return StatusCode(response.Status, new { response });
    }

    [HttpPut("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> UpdateEducation([FromBody] ExperienceDTO dto, [FromRoute] int id, IUserInfo userInfo)
    {
        var response = await workflow.Update(dto, id, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }
}
