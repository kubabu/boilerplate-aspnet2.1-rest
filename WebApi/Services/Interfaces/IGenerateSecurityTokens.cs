using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Services.Interfaces
{
    public interface IGenerateSecurityTokens
    {
        JwtSecurityToken GenerateSecurityToken(Claim[] claims);
    }
}
