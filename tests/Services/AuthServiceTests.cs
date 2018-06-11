//using Microsoft.EntityFrameworkCore;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using WebApi.Models;
//using WebApi.Models.DbContexts;
//using WebApi.Services;
//using WebApi.Services.Interfaces;

//namespace WebApiTests.Services
//{
//    public class AuthServiceTests
//    {
//        string _username;
//        bool _ifPass;
//        Mock<ICheckPasswordService> _passwordService;
//        IAuthorizeService _authorizeService;

//        [SetUp]
//        public void Setup()
//        {
//            _username = "FooBar";
//            var users = new List<User>()
//            {
//                new User() {
//                    Name = _username
//                },
//            };
//            var set = new Mock<DbSet<User>>();
//            Mock<MainDbContext> dbMock = new Mock<MainDbContext>(Mock.Of< DbContextOptions<MainDbContext>>()); //dbContext, ICheckPasswordService checkPasswordService

//            _passwordService = new Mock<ICheckPasswordService>();
//            _passwordService.Setup(p => p.IsPasswordValidForUser(It.IsAny<User>(), It.IsAny<string>()))
//                .Returns(_ifPass);
//            _authorizeService = new AuthService(dbMock.Object, _passwordService.Object);
//        }

//        [Test]
//        public async Task IsValidPassword_invalid()
//        {
//            // setup
//            _ifPass = false;
//            // act
//            var result = await _authorizeService.AuthorizeWithLoginAndPasswordAsync(_username, "invalid password");
//            // verify
//            Assert.Null(result);
//        }

//        [Test]
//        public async Task IsValidPassword_valid()
//        {
//            // setup
//            _ifPass = true;
//            // act
//            var result = await _authorizeService.AuthorizeWithLoginAndPasswordAsync(_username, "valid password");
//            // verify
//            Assert.NotNull(result);
//            Assert.Equals(result.Name, _username);
//        }
//    }
//}
