using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        ILogger<AuthService> _logger;

        
        public AuthService(MainDbContext dbContext, ICheckPasswordService checkPasswordService, ILogger<AuthService> logger)
        {
            _context = dbContext;
            _checkPasswordService = checkPasswordService;
            _logger = logger;
        }

        public async Task<UserViewModel> AuthorizeWithLoginAndPasswordAsync(string login, string password)
        {
            var user = await _context.Users
                .Where(u => u.Name == login)
                .FirstOrDefaultAsync();

            if (user != null && _checkPasswordService.IsPasswordValidForUser(user, password))
            {
                var result = new UserViewModel(user);
                result.Tabs = new List<string>() { "Admin" };

                return result;
            }
            return null;
        }

        public Claim[] GetClaims(UserViewModel user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            return claims;
        }
    }
}
