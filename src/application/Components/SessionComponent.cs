using application.Utils;
using domain.Abstractions;
using domain.Models;
using domain.Specifications.Auth;
using System.Threading.Tasks;
using common.DTO;
using application.Workflows;
using domain.Specifications.Company;

namespace application.Components;

public class SessionComponent(
    IRepository<AuthModel> authRepository,
    IRepository<CompanyModel> companyRepository,
    TokenPublisher tokenPublisher) : Responder
{
    public async Task<Response> RefreshJsonWebToken(string refresh)
    {
        var spec = new AuthByValueSpec(refresh) { Expressions = [x => x.User] };
        var model = await authRepository.GetByFilterAsync(spec);

        if (model is null || model.User is null)
            return Response(404);

        var company = await companyRepository.GetByFilterAsync(new CompanyByRelationSpec(model.UserId));
        int? companyId = company?.CompanyId;

        var jwt = tokenPublisher.JsonWebToken(new JwtDTO
        {
            Expires = Immutable.JwtExpires,
            UserId = model.UserId,
            CompanyId = companyId,
            Role = model.User.Role
        });

        return Response(200, new { jwt });
    }
}
