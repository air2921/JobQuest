using application.Utils;
using common.Exceptions;
using domain.Abstractions;
using domain.Models;
using domain.Specifications.Auth;
using System.Threading.Tasks;
using common.DTO;
using application.Workflows;

namespace application.Components;

public class SessionComponent(IRepository<AuthModel> repository, TokenPublisher tokenPublisher)
{
    public async Task<Response> RefreshJsonWebToken(string refresh)
    {
        var spec = new AuthByValueSpec(refresh) { Expressions = [x => x.User] };
        var model = await repository.GetByFilterAsync(spec);

        if (model is null || model.User is null)
            return new Response { Status = 404 };

        var jwt = tokenPublisher.JsonWebToken(new JwtDTO
        {
            Expires = Immutable.JwtExpires,
            UserId = model.UserId,
            Role = model.User.Role
        });

        return new Response { Status = 200, ObjectData = new { jwt } };
    }
}
