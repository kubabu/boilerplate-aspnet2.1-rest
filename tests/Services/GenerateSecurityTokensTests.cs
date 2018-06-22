using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models.Configuration;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApiTests.Services
{
    public class GenerateSecurityTokensTests
    {
        string _name = "Name";
        string _role = "Role";
        Claim[] _claims;

        private IGenerateSecurityTokens _service;
        JwtSettings _settings;



        [SetUp]
        public void Setup()
        {
            _claims = new[]
            {
                new Claim(ClaimTypes.Name, _name),
                new Claim(ClaimTypes.Role, _role)
            };
            _settings = new JwtSettings() {
                JwtKey = "#SuperSecret123",
                Audience = "Oh my!",
                LifetimeMinutes = 20,
            };
            WebApiSettings apiSettings = new WebApiSettings() { JwtSettings = _settings };

            _service = new GenerateSecurityTokens(apiSettings);
        }

        [Test]
        public void IsGenerated()
        {
            var token = _service.GenerateSecurityToken(_claims);

            // verify
            Assert.NotNull(token);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var tokenHandler = new JwtSecurityTokenHandler();
            //var validationParameters = new TokenValidationParameters
            //{
            //    //NameClaimType = username,
            //    ValidateIssuer = true,
            //    ValidateAudience = true,
            //    ValidateLifetime = true,
            //    ValidateIssuerSigningKey = true,
            //    ValidIssuer = _settings.Issuer,
            //    ValidAudience = _settings.Issuer,
            //    IssuerSigningKey = new SymmetricSecurityKey(_settings.JwtKeyBytes)
            //};

            if (tokenHandler.CanReadToken(tokenString))
            {
                var res = tokenHandler.ValidateToken(tokenString, _settings.GetTokenValidationParameters(), out var rawToken);
                var validatedToken = (JwtSecurityToken)rawToken;
                if (rawToken != null)  // TODO read name claim, compare with 
                {
                    Assert.NotNull(tokenString);
                }
            }
        }
    }
}
