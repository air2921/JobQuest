using application.Utils;
using domain.Abstractions;
using domain.Models;
using domain.Specifications.Auth;
using System.Threading.Tasks;
using common.DTO;
using application.Workflows;

namespace application.Components;

public class SessionComponent(
    IRepository<AuthModel> authRepository,
    TokenPublisher tokenPublisher) : Responder
{
    public async Task<Response> RefreshJsonWebToken(string refresh)
    {
        var spec = new AuthByValueSpec(refresh);
        var model = await authRepository.GetByFilterAsync(spec, [x => x.User]);

        if (model is null || model.User is null)
            return Response(404);

        var jwt = tokenPublisher.JsonWebToken(new JwtDTO
        {
            Expires = Immutable.JwtExpires,
            UserId = model.UserId,
            Role = model.User.Role
        });

        return Response(200, new { jwt });
    }
}
