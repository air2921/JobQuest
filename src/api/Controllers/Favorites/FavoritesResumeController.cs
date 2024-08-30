using api.Utils;
using application.Workflows.Core.Favorites;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace api.Controllers.Favorites;

[Route("api/favorites/resumes")]
[ApiController]
[Authorize(Policy = ApiSettings.EMPLOYER_POLICY)]
public class FavoritesResumeController(FavoritesResumeWk workflow, IUserInfo userInfo) : ControllerBase
{
    [HttpGet("range")]
    public async Task<IActionResult> GetFavorites([FromQuery] int skip, [FromQuery] int count, [FromQuery] bool byDesc)
    {
        var response = await workflow.GetRange(new PaginationDTO { Skip = skip, Total = count, ByDesc = byDesc }, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpGet("{favoriteId}")]
    public async Task<IActionResult> GetFavorite([FromRoute] int favoriteId)
    {
        var response = await workflow.GetSingle(favoriteId, userInfo.UserId);
        return StatusCode(response.Status, new { response });
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteFavorites([FromBody] IEnumerable<int> ids)
    {
        var response = await workflow.RemoveRange(ids, userInfo.UserId);
        return StatusCode(response.Status);
    }

    [HttpDelete("{favoriteId}")]
    public async Task<IActionResult> DeleteFavorite([FromRoute] int favoriteId)
    {
        var response = await workflow.RemoveSingle(favoriteId, userInfo.UserId);
        return StatusCode(response.Status);
    }

    [HttpPost("range")]
    public async Task<IActionResult> AddFavorites([FromBody] IEnumerable<int> ids)
    {
        var response = await workflow.AddRange(ids, userInfo.UserId);
        return StatusCode(response.Status);
    }

    [HttpPost("{favoriteId}")]
    public async Task<IActionResult> AddFavorite([FromRoute] int favoriteId)
    {
        var response = await workflow.AddSingle(favoriteId, userInfo.UserId);
        return StatusCode(response.Status);
    }
}
