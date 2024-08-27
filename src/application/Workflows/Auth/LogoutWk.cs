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
    ILocalizer localizer)
{
    public async Task<Response> Logout(string refresh)
    {
        try
        {
            var model = await repository.DeleteByFilterAsync(new AuthByValueSpec(refresh));
            if (model is null)
                return new Response { Status = 404, Message = localizer.Translate(Message.NOT_FOUND) };

            return new Response { Status = 204 };
        }
        catch (EntityException ex)
        {
            return new Response { Status = 500, Message = ex.Message };
        }
    }
}
