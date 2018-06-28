using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Configuration;
using WebApi.Models.DbContexts;
using WebApi.Repositories.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class AuthorizeUsersService: IAuthorizeUsersService
    {
        private readonly ICheckPasswordService _checkPasswordService;
        private readonly ICheckSecurityTokens _checkSecurityTokens;
        private readonly IServeUsers _serveUsers;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthorizeUsersService> _logger;


        public AuthorizeUsersService(IServeUsers serveUsers,
            ICheckPasswordService checkPasswordService,
            ICheckSecurityTokens checkSecurityTokens,
            ILogger<AuthorizeUsersService> logger,
            WebApiSettings settings)
        {
            _serveUsers = serveUsers;
            _checkPasswordService = checkPasswordService;
            _checkSecurityTokens = checkSecurityTokens;
            _logger = logger;
            _jwtSettings = settings.JwtSettings;
        }

        
        public async Task<AuthorizedUser> AuthorizeWithLoginAndPasswordAsync(TokenIssueRequest issueRequest)
        {
            var user = await _serveUsers.GetUserByName(issueRequest.Username);

            if (user != null && _checkPasswordService.IsPasswordValidForUser(user, issueRequest.Password))
            {
                var result = new AuthorizedUser(user);     
                return result;
            }
            return null;
        }

        public async Task<AuthorizedUser> AuthorizeUserWithTokenAsync(TokenReissueRequest reissueRequest)
        {
            var user = await _serveUsers.GetUserByName(reissueRequest.Username);

            if (user != null && _checkSecurityTokens.IsValidForUser(user, reissueRequest.Token))
            {
                var result = new AuthorizedUser(user);
                return result;
            }

            return null;
        }
    }
}
