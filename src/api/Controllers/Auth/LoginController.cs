﻿using application.Utils;
using application.Workflows.Auth;
using common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Auth;

[Route("api/login")]
[ApiController]
public class LoginController(LoginWk workflow) : ControllerBase
{
    [HttpPost("initiate")]
    public async Task<IActionResult> Initiate([FromBody] LoginDTO dto)
    {
        var response = await workflow.Initiate(dto);
        return StatusCode(response.Status, new { response });
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromQuery] int code, [FromQuery] string token)
    {
        var response = await workflow.Complete(code, token);
        if (!response.IsSuccess || response.ObjectData is not AuthResponse auth)
            return StatusCode(response.Status, new { response });

        SetCookie(auth);
        return StatusCode(200, new {
            jwt = new { auth.auth, expires = Immutable.JwtExpires },
            refresh = new { auth.refresh, expires = Immutable.RefreshExpires }
        });
    }

    private void SetCookie(AuthResponse auth)
    {
        HttpContext.Response.Cookies.Append(Immutable.JWT_COOKIE_KEY, auth.auth, SetOptions(Immutable.JwtExpires));
        HttpContext.Response.Cookies.Append(Immutable.JWT_COOKIE_KEY, auth.refresh, SetOptions(Immutable.RefreshExpires));
    }

    private static CookieOptions SetOptions(TimeSpan expires)
    {
        return new CookieOptions
        {
            MaxAge = expires,
            Secure = true,
            HttpOnly = false,
            SameSite = SameSiteMode.None,
            IsEssential = false
        };
    }

    private class AuthResponse
    {
        public string refresh { get; set; } = null!;
        public string auth { get; set; } = null!;
    }
}
