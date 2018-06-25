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
        private readonly IAuthorizeUsersService _authService;
        private readonly IPrepareTokenResponse _responseService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthorizeUsersService authorizeService,
            IPrepareTokenResponse responseService,
            ILogger<AuthController> logger)
        {
            _authService = authorizeService;
            _responseService = responseService;
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
                if (ex is InvalidOperationException && ex.InnerException is TimeoutException)
                {
                    _logger.LogError("TimeoutException when trying to connect with DB");
                    return StatusCode(500, "Serwer nie otrzymał odpowiedzi z bazy danych, skontaktuj się z administratorem");
                }
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
            var response = _responseService.PrepareToken(user);
            
            return Ok(new
            {
                token = response.Token,
                validTo = response.ValidTo,
                user
            });
        }
    }
}
