using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models.Configuration
{
    public class JwtSettings
    {
        public string JwtKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int LifetimeMinutes { get; set; }

        public Byte[] JwtKeyBytes { get => Encoding.UTF8.GetBytes(JwtKey); }

        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Issuer,
                ValidAudience = Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(JwtKeyBytes)
            };
        }
    }
}
