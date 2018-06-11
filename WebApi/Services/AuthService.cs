using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        ILogger<AuthService> _logger;

        
        public AuthService(MainDbContext dbContext, ICheckPasswordService checkPasswordService, ILogger<AuthService> logger)
        {
            _context = dbContext;
            _checkPasswordService = checkPasswordService;
            _logger = logger;
        }

        public async Task<ClientUser> AuthorizeWithLoginAndPasswordAsync(string login, string password)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Name == login)
                    .FirstOrDefaultAsync();

                if (user != null && _checkPasswordService.IsPasswordValidForUser(user, password))
                {
                    return new ClientUser(user);
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("Auth error: {0} ({1})", ex.Message, ex.InnerException);
            }
            return null;
        }
    }
}
