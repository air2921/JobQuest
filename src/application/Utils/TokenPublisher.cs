using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using domain.Abstractions;
using common.DTO;
using common;
using System.Collections.Generic;

namespace application.Utils;

public class TokenPublisher(IConfiguration configuration, IGenerate generate)
{
    public virtual string JsonWebToken(JwtDTO dto)
    {
        var section = configuration.GetSection(App.JWT_SECTION);

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(section[App.SECRET_KEY]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, dto.UserId.ToString()),
            new(ClaimTypes.Role, dto.Role)
        };

        var token = new JwtSecurityToken(
            issuer: section[App.ISSUER]!,
            audience: section[App.AUDIENCE],
            claims: claims,
            expires: DateTime.UtcNow + dto.Expires,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public virtual string RefreshToken() => generate.GuidCombine(3);
}
