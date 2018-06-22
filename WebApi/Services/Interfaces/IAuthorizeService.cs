using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IAuthorizeService
    {
        Task<AuthorizedUser> AuthorizeWithLoginAndPasswordAsync(TokenIssueRequest issueRequest);
        Task<AuthorizedUser> AuthorizeUserWithToken(TokenReissueRequest reissueRequest);
        Claim[] GetClaims(AuthorizedUser user);
        AuthorizationResult PrepareToken(AuthorizedUser user);
    }
}
