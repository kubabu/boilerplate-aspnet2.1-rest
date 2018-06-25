using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models;
using WebApi.Models.Configuration;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApiTests.Services
{
    public class TokenResponseServiceTests
    {
        AuthorizedUser user;
        JwtSecurityToken token;
        string writtenToken = "abc 123 $%$%^%^";
        Mock<IGenerateSecurityTokens> generateTokensServiceMock;
        WebApiSettings settings;
        TokenResponseService _service;


        [SetUp]
        public void Setup()
        {
            token = new JwtSecurityToken();
            generateTokensServiceMock = new Mock<IGenerateSecurityTokens>();
            generateTokensServiceMock
                .Setup(a => a.GenerateSecurityToken(
                    It.IsAny<Claim[]>(),
                    It.IsAny<JwtSettings>(),
                    It.IsAny<DateTime>()))
                .Returns(() => token);
            generateTokensServiceMock
                .Setup(a => a.WriteToken(It.IsAny<JwtSecurityToken>()))
                .Returns(() => writtenToken);
            settings = new WebApiSettings()
            {
                JwtSettings = new JwtSettings()
            };

            _service = new TokenResponseService(
                generateTokensServiceMock.Object,
                Mock.Of<ILogger<TokenResponseService>>(),
                settings);
            
            user = new AuthorizedUser(new User())
            {
                Name = "Name",
                Role = "Role"
            };
        }

        [Test]
        public void ClaimsTest()
        {
            // act
            var claims = _service.GetClaims(user);

            // verify
            claims.Should().NotBeNullOrEmpty();
            claims.Length.Should().Be(2);
            var verifiedClaims = 0;
            var expectedClaims = 2;
            foreach(var claim in claims)
            {
                if ((claim.Type == ClaimTypes.Name && claim.Value == user.Name) 
                    || (claim.Type == ClaimTypes.Role && claim.Value == user.Role))
                {
                    verifiedClaims++;
                }
            }
            verifiedClaims.Should().Be(expectedClaims);
        }

        [Test]
        public void ResponseTest()
        {
            var result = _service.PrepareToken(user);

            result.Token.Should().Be(writtenToken);
            result.User.Should().Be(user);
            result.ValidTo.Should().Be(token.ValidTo);
        }
    }
}
