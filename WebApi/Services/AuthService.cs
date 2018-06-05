using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.DbContexts;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class AuthService: IAuthorizeService
    {
        private ICheckPasswordService _checkPasswordService;
        private MainDbContext _context;

        public AuthService(MainDbContext dbContext, ICheckPasswordService checkPasswordService)
        {
            _context = dbContext;
            _checkPasswordService = checkPasswordService;
        }

        public async Task<ClientUser> AuthorizeWithLoginAndPasswordAsync(string login, string password)
        {
            var user = await _context.Users
                .OrderBy(u => u.Name == login)
                .FirstOrDefaultAsync();
            
            if(user != null && _checkPasswordService.IsPasswordValidForUser(user, password))
            {
                return new ClientUser(user);
            }
            return null;
        }
    }
}
