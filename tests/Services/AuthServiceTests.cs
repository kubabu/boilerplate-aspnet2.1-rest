using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApiTests.Services
{
    public class AuthServiceTests
    {
        IAuthorizeService _authorizeService;

        [SetUp]
        public void Setup()
        {

            //_authorizeService = new AuthService();
        }

        [Test]
        public void IsValidPassword()
        {
        }
    }
}
