using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.Specifications.Auth;
using JsonLocalizer;
using System.Threading.Tasks;

namespace application.Workflows.Auth;

public class LogoutWk(
    IRepository<AuthModel> repository,
    ILocalizer localizer) : Responder
{
    public async Task<Response> Logout(string refresh)
    {
        try
        {
            var model = await repository.DeleteByFilterAsync(new AuthByValueSpec(refresh));
            if (model is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(404);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }
}
