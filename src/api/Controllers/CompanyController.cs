﻿using api.Utils;
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
        => this.Response(await workflow.AddSingle(dto, userInfo.UserId));

    [HttpDelete("{id}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> DeleteCompany([FromRoute] int id, IUserInfo userInfo)
        => this.Response(await workflow.RemoveSingle(id, userInfo.UserId));

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCompany([FromRoute] int id)
        => this.Response(await workflow.GetSingle(id));

    [HttpPost("get")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCompanies([FromBody] SortCompanyDTO dto)
        => this.Response(await workflow.GetRange(dto));

    [HttpPut("{id}")]
    [Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
    public async Task<IActionResult> UpdateCompany([FromBody] CompanyDTO dto, [FromRoute] int id, IUserInfo userInfo)
        => this.Response(await workflow.Update(dto, id, userInfo.UserId));
}
