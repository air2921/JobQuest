using api.Utils;
using application.Workflows.Core;
using common.DTO.ModelDTO;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/vacancies")]
[ApiController]
public class VacancyController(VacancyWk workflow) : ControllerBase
{
    [HttpPost("add")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> AddVacancy([FromBody] VacancyDTO dto, IUserInfo userInfo)
    {
        var response = await workflow.AddSingle(dto, userInfo.CompanyId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("add/range")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> AddVacancies([FromBody] IEnumerable<VacancyDTO> dtos, IUserInfo userInfo)
    {
        var response = await workflow.AddRange(dtos, userInfo.CompanyId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> DeleteVacancy([FromRoute] int id, IUserInfo userInfo)
    {
        var response = await workflow.RemoveSingle(id, userInfo.CompanyId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> DeleteVacancies([FromBody] IEnumerable<int> ids, IUserInfo userInfo)
    {
        var response = await workflow.RemoveRange(ids, userInfo.CompanyId);
        return StatusCode(response.Status, new { response });
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVacancy([FromRoute] int id)
    {
        var response = await workflow.GetSingle(id);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("get")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVacancies([FromBody] SortVacancyDTO dto)
    {
        var response = await workflow.GetRange(dto);
        return StatusCode(response.Status, new { response });
    }

    [HttpPut("{id}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> UpdateVacancy([FromBody] VacancyDTO dto, [FromRoute] int id, IUserInfo userInfo)
    {
        var response = await workflow.Update(dto, id, userInfo.CompanyId);
        return StatusCode(response.Status, new { response });
    }
}
