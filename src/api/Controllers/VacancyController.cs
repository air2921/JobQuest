using api.Utils;
using application.Workflows.Core;
using common.DTO.ModelDTO;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/vacancies")]
[ApiController]
public class VacancyController(VacancyWk workflow) : ControllerBase
{
    [HttpPost("add/{companyId}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> AddVacancy([FromBody] VacancyDTO dto, [FromRoute] int companyId, IUserInfo userInfo)
    {
        var response = await workflow.AddSingle(dto, companyId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("add/range/{companyId}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> AddVacancies([FromBody] IEnumerable<VacancyDTO> dtos, [FromRoute] int companyId, IUserInfo userInfo)
    {
        var response = await workflow.AddRange(dtos, companyId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete("{companyId}/{vacancyId}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> DeleteVacancy([FromRoute] int companyId, [FromRoute] int vacancyId, IUserInfo userInfo)
    {
        var response = await workflow.RemoveSingle(vacancyId, companyId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpGet("{vacancyId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVacancy([FromRoute] int vacancyId)
    {
        var response = await workflow.GetSingle(vacancyId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("get")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVacancies([FromBody] SortVacancyDTO dto)
    {
        var response = await workflow.GetRange(dto);
        return StatusCode(response.Status, new { response });
    }

    [HttpPut("{companyId}/{vacancyId}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> UpdateVacancy([FromBody] VacancyDTO dto, [FromRoute] int companyId, [FromRoute] int vacancyId, IUserInfo userInfo)
    {
        var response = await workflow.Update(dto, vacancyId, companyId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }
}
