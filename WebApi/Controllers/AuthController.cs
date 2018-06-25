using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Services.Interfaces;


namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IAuthorizeUsersService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthorizeUsersService authorizeService, ILogger<AuthController> logger)
        {
            _authService = authorizeService;
            _logger = logger;
        }

        // POST: api/auth
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostTokenRequest([FromBody] TokenIssueRequest request)
        {
            try
            {
                var user = await _authService.AuthorizeWithLoginAndPasswordAsync(request);

                if (user != null)
                {
                    return AuthTokenResponse(user);
                }

                return NotFound("Błędny login lub hasło");
            }
            catch (Exception ex)
            {
                return BadRequest("Hasło jest nieprawidłowe, skontaktuj się z administratorem");
            }
        }

        // POST: api/auth/reissue
        [Authorize]
        [HttpPost("reissue")]
        public async Task<IActionResult> PostTokenReissueRequest([FromBody] TokenReissueRequest reissueRequest)
        {
            var user = await _authService.AuthorizeUserWithToken(reissueRequest);
            if (user != null)
            {
                return AuthTokenResponse(user);
            }
            return BadRequest("");
        }

        private IActionResult AuthTokenResponse(AuthorizedUser user)
        {
            var response = _authService.PrepareToken(user);
            
            return Ok(new
            {
                token = response.Token,
                validTo = response.ValidTo,
                user
            });
        }
    }
}
