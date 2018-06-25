using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Configuration;
using WebApi.Models.DbContexts;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class AuthorizeUsersService: IAuthorizeUsersService
    {
        private ICheckPasswordService _checkPasswordService;
        private ICheckSecurityTokens _checkSecurityTokens;
        private MainDbContext _context;
        private JwtSettings _jwtSettings;
        ILogger<AuthorizeUsersService> _logger;


        public AuthorizeUsersService(MainDbContext dbContext,
            ICheckPasswordService checkPasswordService,
            ICheckSecurityTokens checkSecurityTokens,
            ILogger<AuthorizeUsersService> logger,
            WebApiSettings settings)
        {
            _context = dbContext;
            _checkPasswordService = checkPasswordService;
            _checkSecurityTokens = checkSecurityTokens;
            _logger = logger;
            _jwtSettings = settings.JwtSettings;
        }

        
        public async Task<AuthorizedUser> AuthorizeWithLoginAndPasswordAsync(TokenIssueRequest issueRequest)
        {
            var user = await GetUserByName(issueRequest.Username);

            if (user != null && _checkPasswordService.IsPasswordValidForUser(user, issueRequest.Password))
            {
                var result = new AuthorizedUser(user);     
                return result;
            }
            return null;
        }

        public async Task<AuthorizedUser> AuthorizeUserWithTokenAsync(TokenReissueRequest reissueRequest)
        {
            var user = await GetUserByName(reissueRequest.Username);

            if (user != null && _checkSecurityTokens.IsValidForUser(user, reissueRequest.Token))
            {
                var result = new AuthorizedUser(user);
                return result;
            }

            return null;
        }

        private async Task<User> GetUserByName(string username)
        {
            return await _context.Users
                .Where(u => u.Name == username)
                .FirstOrDefaultAsync();
        }
    }
}
