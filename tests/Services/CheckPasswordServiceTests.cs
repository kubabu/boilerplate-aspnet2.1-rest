using FluentAssertions;
using NUnit.Framework;
using WebApi.Models;
using WebApi.Services;

namespace WebApiTests.Services
{
    public class CheckPasswordServiceTests
    {
        private CheckPasswordService _service;
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
            _service.IsValidPassword(_pass, _hashed).Should().BeTrue();
            user.Password = _hashed;

            _service.IsPasswordValidForUser(user, _pass).Should().BeTrue();
        }
    }
}
