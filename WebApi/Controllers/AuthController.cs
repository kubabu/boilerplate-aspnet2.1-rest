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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IAuthorizeService _authService;
        private WebApiSettings _settings;

        public AuthController(IAuthorizeService authorizeService, WebApiSettings settings)
        {
            _authService = authorizeService;
            _settings = settings;
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
                    new Claim(ClaimTypes.Name, request.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtSettings.JwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _settings.JwtSettings.Issuer,
                    audience: _settings.JwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

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
