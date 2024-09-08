using api.Utils;
using application.Workflows.Core.Favorites;
using domain.SpecDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Favorites;

[Route("api/favorites/vacancies")]
[ApiController]
[Authorize(Policy = ApiSettings.APPLICANT_POLICY)]
public class FavoritesVacancyController(FavoritesVacancyWk workflow, IUserInfo userInfo) : ControllerBase
{
    [HttpGet("range")]
    public async Task<IActionResult> GetFavorites([FromQuery] int skip, [FromQuery] int count, [FromQuery] bool byDesc)
        => this.Response(await workflow.GetRange(new PaginationDTO { Skip = skip, Total = count, ByDesc = byDesc }, userInfo.UserId));

    [HttpGet("{favoriteId}")]
    public async Task<IActionResult> GetFavorite([FromRoute] int favoriteId)
        => this.Response(await workflow.GetSingle(favoriteId, userInfo.UserId));

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteFavorites([FromBody] IEnumerable<int> ids)
        => this.Response(await workflow.RemoveRange(ids, userInfo.UserId));

    [HttpDelete("{favoriteId}")]
    public async Task<IActionResult> DeleteFavorite([FromRoute] int favoriteId)
        => this.Response(await workflow.RemoveSingle(favoriteId, userInfo.UserId));

    [HttpPost("range")]
    public async Task<IActionResult> AddFavorites([FromBody] IEnumerable<int> ids)
        => this.Response(await workflow.AddRange(ids, userInfo.UserId));

    [HttpPost("{favoriteId}")]
    public async Task<IActionResult> AddFavorite([FromRoute] int favoriteId)
        => this.Response(await workflow.AddSingle(favoriteId, userInfo.UserId));
}
