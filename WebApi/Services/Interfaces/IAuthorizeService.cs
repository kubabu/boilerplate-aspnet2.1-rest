using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IAuthorizeService
    {
        User AuthorizeWithLoginAndPassword(string login, string password);
        Task<User> AuthorizeWithLoginAndPasswordAsync(string login, string password);
    }
}
