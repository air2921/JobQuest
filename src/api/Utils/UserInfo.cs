﻿using System.Security.Claims;

namespace api.Utils;

public class UserInfo(IHttpContextAccessor httpContextAccessor) : IUserInfo
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext ??
        throw new InvalidOperationException("It is not allowed to use HttpContext to retrieve user data for unauthorized access");

    public int UserId => GetIntClaimValue(ClaimTypes.NameIdentifier);

    public string Role => GetStringClaimValue(ClaimTypes.Role);

    private string GetStringClaimValue(string claimType)
    {
        ClaimsPrincipal user = _httpContext.User;
        string? value = user.FindFirstValue(claimType);
        return value ??
            throw new ArgumentNullException(claimType, $"Cannot retrieve claim value for {claimType} as string");
    }

    private int GetIntClaimValue(string claimType)
    {
        ClaimsPrincipal user = _httpContext.User;
        string? value = user.FindFirstValue(claimType);
        if (value is not null && int.TryParse(value, out int result))
            return result;
        else
            throw new ArgumentNullException(claimType, $"Cannot retrieve claim value for {claimType} as integer");
    }
}

public interface IUserInfo
{
    public int UserId { get; }
    public string Role { get; }
}
