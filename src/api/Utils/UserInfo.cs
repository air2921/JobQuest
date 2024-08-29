using System.Security.Claims;

namespace api.Utils;

public class UserInfo(IHttpContextAccessor httpContextAccessor) : IUserInfo
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("User is not authenticated");

    public int UserId => GetIntClaimValue(ClaimTypes.NameIdentifier);

    public int? CompanyId => GetNullableIntValue(ClaimTypes.UserData);

    public string Role => GetStringClaimValue(ClaimTypes.Role);

    private string GetStringClaimValue(string claimType)
    {
        ClaimsPrincipal user = _httpContext.User;
        string? value = user.FindFirstValue(claimType);
        if (value is not null)
            return value;
        else
            throw new InvalidOperationException($"Cannot retrieve claim value for {claimType} as string");
    }

    private int GetIntClaimValue(string claimType)
    {
        ClaimsPrincipal user = _httpContext.User;
        string? value = user.FindFirstValue(claimType);
        if (value is not null && int.TryParse(value, out int result))
            return result;
        else
            throw new InvalidOperationException($"Cannot retrieve claim value for {claimType} as integer");
    }

    private int? GetNullableIntValue(string claimType)
    {
        ClaimsPrincipal user = _httpContext.User;
        string? value = user.FindFirstValue(claimType);
        if (value is null)
            return null;

        if (int.TryParse(value, out int result))
            return result;
        else
            throw new InvalidOperationException($"Cannot retrieve claim value for {claimType} as integer");
    }
}

public interface IUserInfo
{
    public int UserId { get; }
    public int? CompanyId { get; }
    public string Role { get; }
}

