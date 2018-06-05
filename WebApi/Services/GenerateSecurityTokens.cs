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
        private JwtSettings _settings;

        public GenerateSecurityTokens(WebApiSettings settings)
        {
            _settings = settings.JwtSettings;
        }

        public JwtSecurityToken GenerateSecurityToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(_settings.JwtKeyBytes);  // todo: maybe put there asymmetric RSA?
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(_settings.LifetimeMinutes);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return token;
        }
    }
}
