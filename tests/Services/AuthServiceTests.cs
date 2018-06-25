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
            passwordService.Setup(p => p.IsPasswordValidForUser(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(() => this._ifPass);

            _authorizeService = new AuthorizeUsersService(_dbContext, 
                passwordService.Object,
                Mock.Of<ILogger<AuthorizeUsersService>>(), 
                new WebApiSettings());
        }

        [Test]
        public async Task IsValidPassword_invalid()
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
        public async Task IsValidPassword_valid()
        {
            // setup
            _ifPass = true;
            _dbContext.Users.Add(new User() { Name = _username });
            await _dbContext.SaveChangesAsync();
            int usersCount = await _dbContext.Users.CountAsync();
            Assert.AreEqual(1, usersCount);
            var request = new TokenIssueRequest() { Username = _username, Password = "valid password" };

            // act
            var result = await _authorizeService.AuthorizeWithLoginAndPasswordAsync(request);
            
            // verify
            Assert.NotNull(result);
            Assert.AreEqual(result.Name, _username);
        }
    }
}
