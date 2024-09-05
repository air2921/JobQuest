using application.Utils;
using domain.Abstractions;
using domain.Models;
using domain.Specifications.Auth;
using System.Threading.Tasks;
using common.DTO;
using application.Workflows;
using domain.Includes;

namespace application.Components;

public class SessionComponent(
    IRepository<AuthModel> authRepository,
    TokenPublisher tokenPublisher) : Responder
{
    public async Task<Response> RefreshJsonWebToken(string refresh)
    {
        var spec = new AuthByValueSpec(refresh);
        var include = new AuthInclude { IncludeUser = true };
        var model = await authRepository.GetByFilterAsync(spec, include);

        if (model is null || model.User is null)
            return Response(404);

        if (model.User.IsBlocked)
            return Response(403);

        var jwt = tokenPublisher.JsonWebToken(new JwtDTO
        {
            Expires = Immutable.JwtExpires,
            UserId = model.UserId,
            Role = model.User.Role
        });

        return Response(200, new { jwt });
    }
}
