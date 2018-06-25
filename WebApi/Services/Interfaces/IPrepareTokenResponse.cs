using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IPrepareTokenResponse
    {
        AuthorizationResult PrepareTokenResponse(AuthorizedUser user);
    }
}
