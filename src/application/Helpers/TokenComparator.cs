﻿using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using domain.Abstractions;
using common.DTO;
using common;
using domain.Models;
using System.Threading.Tasks;
using domain.Specifications.Response;

namespace application.Helpers;

public class HH(IRepository<ResponseModel> repository)
{
    public async Task Gg()
    {
        var spec = new SortResponseSpec(0, 20, true, 2921)
        {
            Expressions = [x => x.Vacancy, x => x.Resume]
        };

        var responses = await repository.GetRangeAsync(spec);
    }
}

public class TokenComparator(IConfiguration configuration, IGenerate generate)
{
    public string JsonWebToken(JwtDTO dto)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration[App.SECRET_KEY]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, dto.Username),
            new Claim(ClaimTypes.NameIdentifier, dto.UserId.ToString()),
            new Claim(ClaimTypes.Role, dto.Role)
        };

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
