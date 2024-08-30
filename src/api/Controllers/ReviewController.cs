using api.Utils;
using application.Workflows.Core;
using common.DTO.ModelDTO;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/reviews")]
[ApiController]
public class ReviewController(ReviewWk workflow) : ControllerBase
{
    [HttpPost("add")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddReview([FromBody] ReviewDTO dto, IUserInfo userInfo)
    {
        var response = await workflow.AddSingle(dto, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("add/range")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> AddReviews([FromBody] IEnumerable<ReviewDTO> dtos, IUserInfo userInfo)
    {
        var response = await workflow.AddRange(dtos, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> DeleteReview([FromRoute] int id, IUserInfo userInfo)
    {
        var response = await workflow.RemoveSingle(id, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> DeleteReviews([FromBody] IEnumerable<int> ids, IUserInfo userInfo)
    {
        var response = await workflow.RemoveRange(ids, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetReview([FromRoute] int id)
    {
        var response = await workflow.GetSingle(id);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("get")]
    [AllowAnonymous]
    public async Task<IActionResult> GetReviews([FromBody] PaginationDTO dto, [FromQuery] int companyId)
    {
        var response = await workflow.GetRange(dto, companyId);
        return StatusCode(response.Status, new { response });
    }

    [HttpPut("{id}")]
    [Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
    public async Task<IActionResult> UpdateReview([FromBody] ReviewDTO dto, [FromRoute] int id, IUserInfo userInfo)
    {
        var response = await workflow.Update(dto, id, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }
}
