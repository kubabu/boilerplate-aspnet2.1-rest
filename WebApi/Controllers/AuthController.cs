using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using WebApi.Models.Configuration;
using WebApi.Services;
using WebApi.Services.Interfaces;


namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IAuthorizeService _authService;
        private IGenerateSecurityTokens _generateTokensService;

        public AuthController(IAuthorizeService authorizeService, IGenerateSecurityTokens generateTokens)
        {
            _authService = authorizeService;
            _generateTokensService = generateTokens;
        }

        // POST: api/auth
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostTokenRequest([FromBody] TokenRequest request)
        {
            var user = await _authService.AuthorizeWithLoginAndPasswordAsync(request.Username, request.Password);

            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Name)
                };
                var token = _generateTokensService.GenerateSecurityToken(claims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    user
                });
            }

            return BadRequest("Could not verify username and password");
        }
    }
}
