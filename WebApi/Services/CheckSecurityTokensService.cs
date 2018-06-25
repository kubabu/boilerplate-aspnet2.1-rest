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
    public class CheckSecurityTokensService : ICheckSecurityTokens
    {
        private readonly JwtSettings _jwtSettings;

        public CheckSecurityTokensService(WebApiSettings settings)
        {
            _jwtSettings = settings.JwtSettings;
        }


        public bool IsValidForUser(IAppUser user, string token)
        {
            var jwtToken = ReadToken(token);
            return IsValidForUser(user, jwtToken);
        }

        public bool IsValidForUser(IAppUser user, JwtSecurityToken token)
        {
            if (token != null && token.Claims != null)
            {
                var claims = token.Claims.GetEnumerator();
                while (claims.MoveNext())
                {
                    var claim = claims.Current;
                    if (claim.Type == ClaimTypes.Name && claim.Value == user.Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // reading tokens and checking them for user are separate, split for DRY if needed
        public JwtSecurityToken ReadToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(token))
            {
                var res = tokenHandler.ValidateToken(token,
                    _jwtSettings.GetTokenValidationParameters(),
                    out var rawToken);
                var validatedToken = (JwtSecurityToken)rawToken;

                return validatedToken;
            }
            return null;
        }
    }
}
