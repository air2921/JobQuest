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
        => this.Response(await workflow.AddSingle(dto, userInfo.UserId));

    [HttpPost("add/range")]
    public async Task<IActionResult> AddLanguages([FromBody] IEnumerable<LanguageDTO> dtos)
        => this.Response(await workflow.AddRange(dtos, userInfo.UserId));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLanguage([FromRoute] int id)
        => this.Response(await workflow.RemoveSingle(id, userInfo.UserId));

    [HttpDelete]
    public async Task<IActionResult> DeleteLanguages([FromBody] IEnumerable<int> ids)
        => this.Response(await workflow.RemoveRange(ids, userInfo.UserId));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLanguage([FromRoute] int id)
        => this.Response(await workflow.GetSingle(id, userInfo.UserId));

    [HttpPost("get")]
    public async Task<IActionResult> GetLanguages([FromBody] SortLanguageDTO dto)
        => this.Response(await workflow.GetRange(dto, userInfo.UserId));

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLanguage([FromBody] LanguageDTO dto, [FromRoute] int id)
        => this.Response(await workflow.Update(dto, id, userInfo.UserId));
}
