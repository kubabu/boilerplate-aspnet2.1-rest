using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Configuration;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class TokenResponseService: IPrepareTokenResponse
    {
        private JwtSettings _jwtSettings;
        private IGenerateSecurityTokens _generateTokensService;
        ILogger<TokenResponseService> _logger;


        public TokenResponseService(IGenerateSecurityTokens generateTokensService,
            ILogger<TokenResponseService> logger,
            WebApiSettings settings)
        {
            _generateTokensService = generateTokensService;
            _logger = logger;
            _jwtSettings = settings.JwtSettings;
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


        public AuthorizationResult PrepareTokenResponse(AuthorizedUser user)
        {
            var claims = GetClaims(user);
            var token = _generateTokensService.GenerateSecurityToken(claims, _jwtSettings, DateTime.Now);
            var writtenToken = _generateTokensService.WriteToken(token);

            return new AuthorizationResult()
            {
                User = user,
                Token = writtenToken,
                ValidTo = token.ValidTo
            };
        }
    }
}
