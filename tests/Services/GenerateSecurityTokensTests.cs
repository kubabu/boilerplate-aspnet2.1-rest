using FluentAssertions;
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
            var somewhen = DateTime.Now;

            // act
            var token = _service.GenerateSecurityToken(_cfg.Claims, _cfg.Settings, somewhen);
            var tokenString = _service.WriteToken(token);

            // verify
            token.Should().NotBeNull();
            tokenString.Should().NotBeNullOrEmpty();
            tokenString.Should().NotBeNullOrWhiteSpace();

            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(tokenString))
            {
                var res = tokenHandler.ValidateToken(tokenString, 
                    _cfg.Settings.GetTokenValidationParameters(),
                    out var rawToken);
                var validatedToken = (JwtSecurityToken)rawToken;

                var claims = validatedToken.Claims.GetEnumerator();
                var verifiedClaims = 0;
                var expectedClaims = 2;
                while (claims.MoveNext())
                {
                    var claim = claims.Current;

                    if ((claim.Type == ClaimTypes.Name && claim.Value == _cfg.Name)
                    || (claim.Type == ClaimTypes.Role && claim.Value == _cfg.Role))
                    {
                        verifiedClaims++;
                    }
                }
                verifiedClaims.Should().Be(expectedClaims);
            }
        }
    }
}
