using api.Utils;
using application.Workflows.Core;
using common.DTO.ModelDTO;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/reviews")]
[ApiController]
public class ReviewController(ReviewWk workflow) : ControllerBase
{
    [HttpPost("add")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddReview([FromBody] ReviewDTO dto, IUserInfo userInfo)
        => this.Response(await workflow.AddSingle(dto, userInfo.UserId));

    [HttpPost("add/range")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddReviews([FromBody] IEnumerable<ReviewDTO> dtos, IUserInfo userInfo)
        => this.Response(await workflow.AddRange(dtos, userInfo.UserId));

    [HttpDelete("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> DeleteReview([FromRoute] int id, IUserInfo userInfo)
        => this.Response(await workflow.RemoveSingle(id, userInfo.UserId));

    [HttpDelete]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> DeleteReviews([FromBody] IEnumerable<int> ids, IUserInfo userInfo)
        => this.Response(await workflow.RemoveRange(ids, userInfo.UserId));

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetReview([FromRoute] int id)
        => this.Response(await workflow.GetSingle(id));

    [HttpPost("get")]
    [AllowAnonymous]
    public async Task<IActionResult> GetReviews([FromBody] PaginationDTO dto, [FromQuery] int companyId)
        => this.Response(await workflow.GetRange(dto, companyId));

    [HttpPut("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> UpdateReview([FromBody] ReviewDTO dto, [FromRoute] int id, IUserInfo userInfo)
        => this.Response(await workflow.Update(dto, id, userInfo.UserId));
}
