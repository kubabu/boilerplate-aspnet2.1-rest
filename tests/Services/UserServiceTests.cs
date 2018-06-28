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
using WebApi.Models.DbContexts;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApiTests.Services
{
    public class UserServiceTests
    {
        bool passwordService_ifPass = true;
        string passwordService_passToReturn = "passwordService_passToReturn";
        MainDbContext _dbContext;
        IServeUsers _service;


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
                .Returns(() => this.passwordService_ifPass);
            passwordService
                .Setup(p => p.HashPassword(It.IsAny<string>()))
                .Returns(() => this.passwordService_passToReturn);

            _service = new UserService(_dbContext,
                passwordService.Object,
                Mock.Of<ILogger<UserService>>());
        }

        [Test]
        public async Task AuthorizeWithLoginAndPassword_valid_authorizedUser()
        {
            // setup
            passwordService_ifPass = true;
            var user = new User() { Name = "Foo", Password = "bar" };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            int usersCount = await _dbContext.Users.CountAsync();
            usersCount.Should().Be(1);

            // act
            var result = await _service.UpdateUserAsync(null);

            // verify
            result.Should().BeTrue();
        }

        // TODO
        // add new user, should add to db
        // add user when user with such name already exist, should throw error
        // change user with new password, verify its changed to hashed by password service
        // change user without new password, verify its untouched
    }
}
