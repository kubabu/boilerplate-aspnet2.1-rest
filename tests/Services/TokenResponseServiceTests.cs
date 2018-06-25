using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;
using WebApi.Models.Configuration;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApiTests.Services
{
    public class TokenResponseServiceTests
    {
        TokenResponseService _service;
        Mock<IGenerateSecurityTokens> generateTokensServiceMock;
        WebApiSettings settings;


        [SetUp]
        public void Setup()
        {
            generateTokensServiceMock = new Mock<IGenerateSecurityTokens>();
            settings = new WebApiSettings()
            {
                JwtSettings = new JwtSettings()
                {
                    JwtKey = "#SuperSecret123",
                    LifetimeMinutes = 20,
                }
            };

            _service = new TokenResponseService(
                generateTokensServiceMock.Object,
                Mock.Of<ILogger<TokenResponseService>>(),
                settings);
        }

        [Test]
        public void ClaimsTest()
        {
            var user = new AuthorizedUser(new User()
            {
                Name = "Name",
            });


            var claims = _service.GetClaims(user);

            claims.Should().NotBeNullOrEmpty();
        }
    }
}
