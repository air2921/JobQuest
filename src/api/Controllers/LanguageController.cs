using api.Utils;
using application.Workflows.Core;
using common.DTO.ModelDTO;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/languages")]
[ApiController]
[Authorize]
public class LanguageController(LanguageWk workflow, IUserInfo userInfo) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> AddLanguage([FromBody] LanguageDTO dto)
    {
        var response = await workflow.AddSingle(dto, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("add/range")]
    public async Task<IActionResult> AddLanguages([FromBody] IEnumerable<LanguageDTO> dtos)
    {
        var response = await workflow.AddRange(dtos, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLanguage([FromRoute] int id)
    {
        var response = await workflow.RemoveSingle(id, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteLanguages([FromBody] IEnumerable<int> ids)
    {
        var response = await workflow.RemoveRange(ids, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLanguage([FromRoute] int id)
    {
        var response = await workflow.GetSingle(id, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetLanguages([FromBody] SortLanguageDTO dto)
    {
        var response = await workflow.GetRange(dto, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLanguage([FromBody] LanguageDTO dto, [FromRoute] int id)
    {
        var response = await workflow.Update(dto, id, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }
}
