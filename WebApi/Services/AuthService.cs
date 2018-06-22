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
    public class AuthService: IAuthorizeService
    {
        private ICheckPasswordService _checkPasswordService;
        private MainDbContext _context;
        private JwtSettings _jwtSettings;
        private IGenerateSecurityTokens _generateTokensService;
        ILogger<AuthService> _logger;

        
        public AuthService(MainDbContext dbContext, 
            ICheckPasswordService checkPasswordService,
            IGenerateSecurityTokens generateTokensService,
            ILogger<AuthService> logger,
            WebApiSettings settings)
        {
            _context = dbContext;
            _checkPasswordService = checkPasswordService;
            _generateTokensService = generateTokensService; 
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
                if (rawToken != null)  // TODO read name claim, compare with 
                {
                    // find user
                    var authUser = new AuthorizedUser(await GetUser(reissueRequest.Username));
                }
                return null;

            }
            throw new NotImplementedException();
        }

        public Claim[] GetClaims(AuthorizedUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            return claims;
        }

        private async Task<User> GetUser(string username)
        {
            return await _context.Users
                .Where(u => u.Name == username)
                .FirstOrDefaultAsync();
        }

        public AuthorizationResult PrepareToken(AuthorizedUser user)
        {
            var claims = GetClaims(user);
            var token = _generateTokensService.GenerateSecurityToken(claims);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthorizationResult()
            {
                User = user,
                Token = tokenString,
                ValidTo = token.ValidTo
            };
        }
    }
}
