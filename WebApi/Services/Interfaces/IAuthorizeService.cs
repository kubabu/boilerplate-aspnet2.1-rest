using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IAuthorizeService
    {
        Task<UserViewModel> AuthorizeWithLoginAndPasswordAsync(string login, string password);
        Claim[] GetClaims(UserViewModel user);
    }
}
