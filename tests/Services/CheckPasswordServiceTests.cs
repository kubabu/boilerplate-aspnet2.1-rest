using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApiTests.Services
{
    public class CheckPasswordServiceTests
    {
        private ICheckPasswordService _service;
        string _pass;
        string _hashed;

        [SetUp]
        public void Setup()
        {
            _service = new CheckPasswordService();
            _pass = "FooBar 123";
            _hashed = _service.HashPassword(_pass);
        }

        [Test]
        public void IsValidPassword()
        {
            Assert.True(_service.IsValidPassword(_pass, _hashed));
        }

        [Test]
        public void IsValidPasswordForUser()
        {
            var user = new User();
            Assert.True(_service.IsValidPassword(_pass, _hashed));
            user.Password = _hashed;

            Assert.True(_service.IsPasswordValidForUser(user, _pass));
        }
    }
}
