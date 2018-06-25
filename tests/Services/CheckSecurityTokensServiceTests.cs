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
    public class CheckSecurityTokensServiceTests
    {
        TokenTestsConfig _cfg;
        DateTime when;
        string tokenString;
        ICheckSecurityTokens _service;


        [SetUp]
        public void Setup()
        {
            _cfg = new TokenTestsConfig();
            when = DateTime.Now;                                    // validateToken uses Now internally to throw out expired tokens
            var tokenGenerator = new GenerateSecurityTokens();      // so generate fresh one everytime
            tokenString = tokenGenerator.WriteToken(tokenGenerator.GenerateSecurityToken(_cfg.Claims, _cfg.JwtSettings, when));

            _service = new CheckSecurityTokensService(_cfg.WebApiSettings);
        }

        [Test]
        public void TokenRead_valid_yes()
        {
            // act
            var token = _service.ReadToken(tokenString);

            // verify
            token.Should().NotBeNull();
        }

        [Test]
        public void TokenRead_invalid_null()
        {
            // act
            var token = _service.ReadToken(tokenString + "sooo invalid now, huh?");

            // verify
            token.Should().BeNull();
        }

        [Test]
        public void TokenRead_empty_null()
        {
            // act
            var token = _service.ReadToken("");

            // verify
            token.Should().BeNull();
        }

        [Test]
        public void TokenForUser_string_yes()
        {
            // act
            var isItOk = _service.IsValidForUser(_cfg.AuthUser, tokenString);

            // verify
            isItOk.Should().BeTrue();
        }

        [Test]
        public void TokenForUser_string_no()
        {
            // act
            var isItOk = _service.IsValidForUser(_cfg.AuthUser, tokenString + "sooo invalid now, huh?");

            // verify
            isItOk.Should().BeFalse();
        }

        [Test]
        public void TokenForUser_emptyString_no()
        {
            // act
            var isItOk = _service.IsValidForUser(_cfg.AuthUser, "");

            // verify
            isItOk.Should().BeFalse();
        }

        [Test]
        public void TokenForUser_JwtToken_yes()
        {
            var token = _service.ReadToken(tokenString);

            // act
            var isItOk = _service.IsValidForUser(_cfg.AuthUser, token);

            // verify
            isItOk.Should().BeTrue();
        }
    }
}
