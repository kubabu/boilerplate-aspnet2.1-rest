using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.Models.Configuration;

namespace WebApi.Services.Interfaces
{
    public interface IGenerateSecurityTokens
    {
        JwtSecurityToken GenerateSecurityToken(Claim[] claims, JwtSettings settings);
    }
}
