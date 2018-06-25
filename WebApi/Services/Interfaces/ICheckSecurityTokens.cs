using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Configuration;

namespace WebApi.Services.Interfaces
{
    public interface ICheckSecurityTokens
    {
        JwtSecurityToken ReadToken(string token);
        bool IsValidForUser(IAppUser user, string token);
        bool IsValidForUser(IAppUser user, JwtSecurityToken token);
    }
}
