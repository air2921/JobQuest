using api.Utils;
using application.Workflows.Core;
using common.DTO.ModelDTO;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/experiences")]
[ApiController]
public class ExperienceController(ExperienceWk workflow) : ControllerBase
{
    [HttpPost("add/{resumeId}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddEducation([FromBody] ExperienceDTO dto, [FromRoute] int resumeId, IUserInfo userInfo)
        => this.Response(await workflow.AddSingle(dto, resumeId, userInfo.UserId));

    [HttpPost("add/range/{resumeId}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddEducations([FromBody] IEnumerable<ExperienceDTO> dtos, [FromRoute] int resumeId, IUserInfo userInfo)
        => this.Response(await workflow.AddRange(dtos, resumeId, userInfo.UserId));

    [HttpDelete("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> DeleteEducation([FromRoute] int id, IUserInfo userInfo)
        => this.Response(await workflow.RemoveSingle(id, userInfo.UserId));

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEducation([FromRoute] int id)
        => this.Response(await workflow.GetSingle(id));

    [HttpPost("get")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEducations([FromBody] SortExperienceDTO dto)
        => this.Response(await workflow.GetRange(dto));

    [HttpPut("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> UpdateEducation([FromBody] ExperienceDTO dto, [FromRoute] int id, IUserInfo userInfo)
        => this.Response(await workflow.Update(dto, id, userInfo.UserId));
}
