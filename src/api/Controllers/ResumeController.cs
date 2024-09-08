using api.Utils;
using application.Workflows.Core;
using common.DTO.ModelDTO;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/resumes")]
[ApiController]
public class ResumeController(ResumeWk workflow) : ControllerBase
{
    [HttpPost("add")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddResume([FromBody] ResumeDTO dto,
        [FromForm] IFormFile? file, IUserInfo userInfo)
        => this.Response(await workflow.AddSingle(dto, userInfo.UserId, file?.OpenReadStream(), file?.FileName));

    [HttpPost("add/range")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddResumes([FromBody] IEnumerable<ResumeDTO> dtos, IUserInfo userInfo)
        => this.Response(await workflow.AddRange(dtos, userInfo.UserId));

    [HttpDelete("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> DeleteResume([FromRoute] int id, IUserInfo userInfo)
        => this.Response(await workflow.RemoveSingle(id, userInfo.UserId));

    [HttpDelete]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> DeleteResumes([FromBody] IEnumerable<int> ids, IUserInfo userInfo)
        => this.Response(await workflow.RemoveRange(ids, userInfo.UserId));

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetResume([FromRoute] int id)
        => this.Response(await workflow.GetSingle(id));

    [HttpPost("get")]
    [AllowAnonymous]
    public async Task<IActionResult> GetResumes([FromBody] SortResumeDTO dto)
        => this.Response(await workflow.GetRange(dto));

    [HttpPut("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> UpdateResume([FromBody] ResumeDTO dto, [FromRoute] int id,
        [FromForm] IFormFile? file, IUserInfo userInfo) 
        => this.Response(await workflow.Update(dto, id, userInfo.UserId, file?.OpenReadStream(), file?.FileName));
}
