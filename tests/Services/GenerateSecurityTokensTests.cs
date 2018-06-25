using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using WebApi.Models.Configuration;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApiTests.Services
{
    public class TokenTestsConfig
    {
        public JwtSettings Settings { get => new JwtSettings()
            {
                JwtKey = "#SuperSecret123 but it needs to be longer than 128bits or something",
                Audience = "Oh my!",
                Issuer = "Oh my!",
                LifetimeMinutes = 20,
            };
        }
        public string Name { get => "User Name to be used in claim tests"; }
        public string Role { get => "Name of Role to be used in claim tests"; }
        public Claim[] Claims { get => new[]
            {
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Role, Role)
            };
        }
    }


    public class GenerateSecurityTokensTests
    {
        private TokenTestsConfig _cfg;
        private IGenerateSecurityTokens _service;

        [SetUp]
        public void Setup()
        {
            _cfg = new TokenTestsConfig();
            _service = new GenerateSecurityTokens();
        }

        [Test]
        public void IsGenerated()
        {
            var somewhen = DateTime.Now; // change it in 100 years or test will fail
            var token = _service.GenerateSecurityToken(_cfg.Claims, _cfg.Settings, somewhen);
            var tokenString = _service.WriteToken(token);

            // verify
            Assert.NotNull(token);
            Assert.IsNotNull(tokenString);

            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(tokenString))
            {
                var valParameters = _cfg.Settings.GetTokenValidationParameters();
                //valParameters.NameClaimType 
                var res = tokenHandler.ValidateToken(tokenString, valParameters, out var rawToken);
                var validatedToken = (JwtSecurityToken)rawToken;

                var claims = validatedToken.Claims.GetEnumerator();
                while(claims.MoveNext())
                {
                    var claim = claims.Current;
                    if (claim.Type == ClaimTypes.Name) // read name claim
                    {
                        if (claim.Value == _cfg.Name)       // com
                        {
                            Assert.NotNull(tokenString);
                        }
                    }
                }

                if (rawToken != null)  // TODO read name claim, compare with 
                {
                    Assert.NotNull(tokenString);
                }
            }
        }
    }
}
