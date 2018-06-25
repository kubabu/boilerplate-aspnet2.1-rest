using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Models.Configuration;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class GenerateSecurityTokens: IGenerateSecurityTokens
    {
        public JwtSecurityToken GenerateSecurityToken(Claim[] claims, JwtSettings settings, DateTime now)
        {
            var key = new SymmetricSecurityKey(settings.JwtKeyBytes);  // todo: maybe put there asymmetric RSA?
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = now.AddMinutes(settings.LifetimeMinutes);

            var token = new JwtSecurityToken(
                issuer: settings.Issuer,
                audience: settings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return token;
        }

        public string WriteToken(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
