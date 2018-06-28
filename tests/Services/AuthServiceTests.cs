using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Configuration;
using WebApi.Models.DbContexts;
using WebApi.Repositories.Interfaces;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApiTests.Services
{
    public class AuthServiceTests
    {
        string _username = "FooBar";
        User _user;
        bool _ifPass;
        IAuthorizeUsersService _authorizeService;

        [SetUp]
        public void Setup()
        {
            var usersRepository = new Mock<IServeUsers>();
            usersRepository.Setup(u => u.GetUserByName(It.IsAny<string>()))
                .Returns(() => Task<User>.Factory.StartNew(() => _user));

            var passwordService = new Mock<ICheckPasswordService>();
            passwordService
                .Setup(p => p.IsPasswordValidForUser(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(() => _ifPass);

            var tokenCheckService = new Mock<ICheckSecurityTokens>();
            tokenCheckService
                .Setup(p => p.IsValidForUser(It.IsAny<IAppUser>(), It.IsAny<string>()))
                .Returns(() => _ifPass);

            _authorizeService = new AuthorizeUsersService(usersRepository.Object,
                passwordService.Object,
                tokenCheckService.Object,
                Mock.Of<ILogger<AuthorizeUsersService>>(),
                new WebApiSettings());
        }


        [Test]
        public async Task AuthorizeWithLoginAndPasswordAsync_noUser_null()
        {
            // setup
            _ifPass = false;
            _user = null;
            var request = new TokenIssueRequest() { Username = "Not existing", Password = "invalid password" };

            // act
            var result = await _authorizeService.AuthorizeWithLoginAndPasswordAsync(request);

            // verify
            Assert.Null(result);
        }

        [Test]
        public async Task AuthorizeWithLoginAndPasswordAsync_invalidPassword_null()
        {
            // setup
            _ifPass = false;
            _user = new User { Name = _username };
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
            _user = new User { Name = _username };
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
            _user = new User { Name = _username };
            var request = new TokenIssueRequest() { Username = _username, Password = "valid password" };

            // act
            var result = await _authorizeService.AuthorizeWithLoginAndPasswordAsync(request);

            // verify
            result.Should().NotBeNull();
            result.Name.Should().Be(_username);
        }

        [Test]
        public async Task AuthorizeWithLoginAndTokenAsync_notExistinguser_null()
        {
            // setup
            _ifPass = false;
            _user = null;
            var request = new TokenReissueRequest() { Username = "Unknown", Token = "invalid token" };

            // act
            var result = await _authorizeService.AuthorizeUserWithTokenAsync(request);

            // verify
            result.Should().BeNull();
        }

        [Test]
        public async Task AuthorizeWithLoginAndTokenAsync_invalidToken_null()
        {
            // setup
            _ifPass = false;
            _user = new User { Name = _username };
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
            _user = new User { Name = _username };
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
            _user = new User { Name = _username };
            _ifPass = true;
            var request = new TokenReissueRequest() { Username = _username, Token = "valid token" };

            // act
            var result = await _authorizeService.AuthorizeUserWithTokenAsync(request);

            // verify
            result.Should().NotBeNull();
            result.Name.Should().Be(_username);
        }
    }
}
