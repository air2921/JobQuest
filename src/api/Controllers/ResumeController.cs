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
    {
        var response = await workflow.AddSingle(dto, userInfo.UserId, file?.OpenReadStream(), file?.FileName);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("add/range")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddResumes([FromBody] IEnumerable<ResumeDTO> dtos, IUserInfo userInfo)
    {
        var response = await workflow.AddRange(dtos, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> DeleteResume([FromRoute] int id, IUserInfo userInfo)
    {
        var response = await workflow.RemoveSingle(id, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> DeleteResumes([FromBody] IEnumerable<int> ids, IUserInfo userInfo)
    {
        var response = await workflow.RemoveRange(ids, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetResume([FromRoute] int id)
    {
        var response = await workflow.GetSingle(id);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("get")]
    [AllowAnonymous]
    public async Task<IActionResult> GetResumes([FromBody] SortResumeDTO dto)
    {
        var response = await workflow.GetRange(dto);
        return StatusCode(response.Status, new { response });
    }

    [HttpPut("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> UpdateResume([FromBody] ResumeDTO dto, [FromRoute] int id,
        [FromForm] IFormFile? file, IUserInfo userInfo)
    {
        var response = await workflow.Update(dto, id, userInfo.UserId, file?.OpenReadStream(), file?.FileName);
        return StatusCode(response.Status, new { response });
    }
}
