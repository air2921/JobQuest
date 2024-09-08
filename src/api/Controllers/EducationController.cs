using api.Utils;
using application.Workflows.Core;
using common.DTO.ModelDTO;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/educations")]
[ApiController]
public class EducationController(EducationWk workflow) : ControllerBase
{
    [HttpPost("add/{resumeId}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddEducation([FromBody] EducationDTO dto, [FromRoute] int resumeId, IUserInfo userInfo)
        => this.Response(await workflow.AddSingle(dto, resumeId, userInfo.UserId));

    [HttpPost("add/range/{resumeId}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddEducations([FromBody] IEnumerable<EducationDTO> dtos, [FromRoute] int resumeId, IUserInfo userInfo)
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
    public async Task<IActionResult> GetEducations([FromBody] SortEducationDTO dto)
        => this.Response(await workflow.GetRange(dto));

    [HttpPut("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> UpdateEducation([FromBody] EducationDTO dto, [FromRoute] int id, IUserInfo userInfo)
        => this.Response(await workflow.Update(dto, id, userInfo.UserId));
}
