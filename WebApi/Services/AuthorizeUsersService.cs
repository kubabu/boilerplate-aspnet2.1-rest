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
        private MainDbContext _context;
        private JwtSettings _jwtSettings;
        ILogger<AuthorizeUsersService> _logger;

        
        public AuthorizeUsersService(MainDbContext dbContext, 
            ICheckPasswordService checkPasswordService,
            ILogger<AuthorizeUsersService> logger,
            WebApiSettings settings)
        {
            _context = dbContext;
            _checkPasswordService = checkPasswordService;
            _logger = logger;
            _jwtSettings = settings.JwtSettings;
        }

        
        public async Task<AuthorizedUser> AuthorizeWithLoginAndPasswordAsync(TokenIssueRequest issueRequest)
        {
            var user = await GetUser(issueRequest.Username);

            if (user != null && _checkPasswordService.IsPasswordValidForUser(user, issueRequest.Password))
            {
                var result = new AuthorizedUser(user);     
                return result;
            }
            return null;
        }

        public async Task<AuthorizedUser> AuthorizeUserWithToken(TokenReissueRequest reissueRequest)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // check if token is valid
            if (tokenHandler.CanReadToken(reissueRequest.Token))
            {
                var res = tokenHandler.ValidateToken(reissueRequest.Token, 
                    _jwtSettings.GetTokenValidationParameters(), 
                    out var rawToken);
                var validatedToken = (JwtSecurityToken)rawToken;
                if (rawToken != null)  // TODO read name claim, compare with request
                {
                    // find user
                    var authUser = new AuthorizedUser(await GetUser(reissueRequest.Username));
                }
                return null;

            }
            throw new NotImplementedException();
        }

        private async Task<User> GetUser(string username)
        {
            return await _context.Users
                .Where(u => u.Name == username)
                .FirstOrDefaultAsync();
        }
    }
}
