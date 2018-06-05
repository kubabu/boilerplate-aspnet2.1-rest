using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using WebApi.Services;
using WebApi.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        //IServeUsers _service;
        private IAuthorizeService _authService;
        private string _sercurityKey = "abcqwertyasdf1234&*&*@#";
        //_configuration["SecurityKey"]

        public AuthController(IAuthorizeService authorizeService)
        {
            _authService = authorizeService;
        }

        // POST: api/auth
        [HttpPost]
        public async Task<IActionResult> PostTokenRequest([FromBody] TokenRequest request)
        {
            if (request.Username == "foo" && request.Password == "bar")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_sercurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "yourdomain.com",
                    audience: "yourdomain.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Could not verify username and password");
        }

        //// GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
