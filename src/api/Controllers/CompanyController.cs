using api.Utils;
using application.Workflows.Core;
using common.DTO.ModelDTO;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/companies")]
[ApiController]
public class CompanyController(CompanyWk workflow) : ControllerBase
{
    [HttpPost("add")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> AddCompany([FromBody] CompanyDTO dto, IUserInfo userInfo)
    {
        var response = await workflow.AddSingle(dto, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> DeleteCompany([FromRoute] int id, IUserInfo userInfo)
    {
        var response = await workflow.RemoveSingle(id, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCompany([FromRoute] int id)
    {
        var response = await workflow.GetSingle(id);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("get")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCompanies([FromBody] SortCompanyDTO dto)
    {
        var response = await workflow.GetRange(dto);
        return StatusCode(response.Status, new { response });
    }

    [HttpPut("{id}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> UpdateCompany([FromBody] CompanyDTO dto, [FromRoute] int id, IUserInfo userInfo)
    {
        var response = await workflow.Update(dto, id, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }
}
