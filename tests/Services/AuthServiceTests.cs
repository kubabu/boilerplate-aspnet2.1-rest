using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Configuration;
using WebApi.Models.DbContexts;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApiTests.Services
{
    public class AuthServiceTests
    {
        string _username = "FooBar";
        bool _ifPass;
        MainDbContext _dbContext;
        IAuthorizeUsersService _authorizeService;

        [SetUp]
        public void Setup()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<MainDbContext>()
                .UseSqlite(connection)
                .Options;
            _dbContext = new MainDbContext(options);
            
            // Create the schema in the database
            using (var context = new MainDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            var passwordService = new Mock<ICheckPasswordService>();
            passwordService
                .Setup(p => p.IsPasswordValidForUser(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(() => this._ifPass);

            var tokenCheckService = new Mock<ICheckSecurityTokens>();
            tokenCheckService
                .Setup(p => p.IsValidForUser(It.IsAny<IAppUser>(), It.IsAny<string>()))
                .Returns(() => this._ifPass);

            _authorizeService = new AuthorizeUsersService(_dbContext,
                passwordService.Object,
                tokenCheckService.Object,
                Mock.Of<ILogger<AuthorizeUsersService>>(), 
                new WebApiSettings());
        }

        [Test]
        public async Task AuthorizeWithLoginAndPasswordAsync_invalidPassword_null()
        {
            // setup
            _ifPass = false;
            var request = new TokenIssueRequest() { Username = _username, Password = "invalid password" };
            
            // act
            var result = await _authorizeService.AuthorizeWithLoginAndPasswordAsync(request);
            
            // verify
            Assert.Null(result);
        }

        [Test]
        public async Task AuthorizeWithLoginAndPasswordAsync_invalidPasswordUsername_null()
        {
            // setup
            _ifPass = false;
            var request = new TokenIssueRequest() { Username = "Not Existing", Password = "invalid password" };

            // act
            var result = await _authorizeService.AuthorizeWithLoginAndPasswordAsync(request);

            // verify
            Assert.Null(result);
        }

        [Test]
        public async Task AuthorizeWithLoginAndPasswordAsync_valid_authorizedUser()
        {
            // setup
            _ifPass = true;
            _dbContext.Users.Add(new User() { Name = _username });
            await _dbContext.SaveChangesAsync();
            int usersCount = await _dbContext.Users.CountAsync();
            usersCount.Should().Be(1);
            var request = new TokenIssueRequest() { Username = _username, Password = "valid password" };

            // act
            var result = await _authorizeService.AuthorizeWithLoginAndPasswordAsync(request);

            // verify
            result.Should().NotBeNull();
            result.Name.Should().Be(_username);
        }


        [Test]
        public async Task AuthorizeWithLoginAndTokenAsync_invalidToken_null()
        {
            // setup
            _ifPass = false;
            var request = new TokenReissueRequest() { Username = _username, Token = "invalid token" };

            // act
            var result = await _authorizeService.AuthorizeUserWithTokenAsync(request);

            // verify
            result.Should().BeNull();
        }

        [Test]
        public async Task AuthorizeWithLoginAndTokenAsync_invalidUsername_null()
        {
            // setup
            _ifPass = false;
            var request = new TokenReissueRequest() { Username = "not present", Token = "invalid token" };

            // act
            var result = await _authorizeService.AuthorizeUserWithTokenAsync(request);

            // verify
            result.Should().BeNull();
        }

        [Test]
        public async Task AuthorizeWithLoginAndTokenAsync_validToken_null()
        {
            // setup
            _ifPass = true;
            _dbContext.Users.Add(new User() { Name = _username });
            await _dbContext.SaveChangesAsync();
            int usersCount = await _dbContext.Users.CountAsync();
            usersCount.Should().Be(1);
            var request = new TokenReissueRequest() { Username = _username, Token = "valid token" };

            // act
            var result = await _authorizeService.AuthorizeUserWithTokenAsync(request);

            // verify
            result.Should().NotBeNull();
            result.Name.Should().Be(_username);
        }
    }
}
