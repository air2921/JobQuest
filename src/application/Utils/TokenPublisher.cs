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
    public string JsonWebToken(JwtDTO dto)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration[App.SECRET_KEY]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, dto.UserId.ToString()),
            new(ClaimTypes.Role, dto.Role)
        };

        if (dto.CompanyId.HasValue && dto.CompanyId is not null)
            claims.Add(new Claim(ClaimTypes.UserData, dto.CompanyId.Value.ToString()));

        var token = new JwtSecurityToken(
            issuer: configuration[App.ISSUER]!,
            audience: configuration[App.AUDIENCE],
            claims: claims,
            expires: DateTime.UtcNow + dto.Expires,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string RefreshToken() => generate.GuidCombine(3);
}
