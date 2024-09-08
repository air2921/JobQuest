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
        => this.Response(await workflow.AddSingle(dto, companyId, userInfo.UserId));

    [HttpPost("add/range/{companyId}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> AddVacancies([FromBody] IEnumerable<VacancyDTO> dtos, [FromRoute] int companyId, IUserInfo userInfo)
        => this.Response(await workflow.AddRange(dtos, companyId, userInfo.UserId));

    [HttpDelete("{companyId}/{vacancyId}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> DeleteVacancy([FromRoute] int companyId, [FromRoute] int vacancyId, IUserInfo userInfo)
        => this.Response(await workflow.RemoveSingle(vacancyId, companyId, userInfo.UserId));

    [HttpGet("{vacancyId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVacancy([FromRoute] int vacancyId)
        => this.Response(await workflow.GetSingle(vacancyId));

    [HttpPost("get")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVacancies([FromBody] SortVacancyDTO dto)
        => this.Response(await workflow.GetRange(dto));

    [HttpPut("{companyId}/{vacancyId}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> UpdateVacancy([FromBody] VacancyDTO dto, [FromRoute] int companyId, [FromRoute] int vacancyId, IUserInfo userInfo)
        => this.Response(await workflow.Update(dto, vacancyId, companyId, userInfo.UserId));
}
