using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.DbContexts;
using WebApi.Repositories;
using WebApi.Services.Interfaces;

namespace WebApiTests.Services
{
    public class UserRepositoryTests
    {
        readonly string passwordService_passToReturn = "passwordService_passToReturn";
        MainDbContext _dbContext;
        UserRepository _service;


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
                .Setup(p => p.HashPassword(It.IsAny<string>()))
                .Returns(() => this.passwordService_passToReturn);

            _service = new UserRepository(_dbContext,
                passwordService.Object,
                Mock.Of<ILogger<UserRepository>>());
        }

        [Test]
        public async Task UpdateUserAsync_ChangedUserButNoPassword_true()
        {
            // change user without new password, verify its untouched
            string originalPassword = "aaa kotki dwa";
            string userName = "TestUser";
            _dbContext.Users.Add(new User()
            {
                Id = 1,
                Name = userName,
                Password = originalPassword,
                StartupUri = "",
                Role = "User"
            });
            await _dbContext.SaveChangesAsync();
            int usersCount = await _dbContext.Users.CountAsync();
            usersCount.Should().Be(1);

            // act
            var user = new User()
            {
                Id = 1,
                StartupUri = "google.com",
                Role = "Admin",
                Password = "",
            };
            //var user = await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(m => m.Name == userName);
            //user.StartupUri = "google.com";
            //user.Role = "Admin";
            //user.Password = "";
            var result = await _service.UpdateUserAsync(user);

            // verify
            result.Should().BeTrue();
            var updated = await _dbContext
                .Users.SingleAsync();
            updated.Role.Should().Be(user.Role);
            updated.StartupUri.Should().Be(user.StartupUri);
            updated.Password.Should().Be(originalPassword);
        }

        [Test]
        public async Task UpdateUserAsync_ChangeUserWithPassword_true()
        {
            // setup
            string originalPassword = "FOO BAR PASSWORD 123";
            var user = new User()
            {
                Id = 0,
                Name = "TestUser",
                Password = originalPassword,
                StartupUri = "",
                Role = "User",
                QrIdentifier = ""
                
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            int usersCount = await _dbContext.Users.CountAsync();
            usersCount.Should().Be(1);

            // act
            user.StartupUri = "google.com";
            user.Role = "Admin";
            user.Password = "Changed";
            user.QrIdentifier = "123";
            user.Name = "NewName";

            var result = await _service.UpdateUserAsync(user);

            // verify
            result.Should().BeTrue();
            var updated = await _dbContext
                .Users.AsNoTracking()
                .SingleOrDefaultAsync(m => m.Name == user.Name);
            updated.Role.Should().Be(user.Role);
            updated.StartupUri.Should().Be(user.StartupUri);
            updated.Name.Should().Be(user.Name);
            updated.QrIdentifier.Should().Be(user.QrIdentifier);
            // verify its changed to hashed by password service
            updated.Password.Should().Be(passwordService_passToReturn);
        }

        [Test]
        public async Task UpdateUserAsync_nonExisting_DbUpdateException()
        {
            // setup
            var user = new User()
            {
                Name = "UserName",
                Password = "Changed",
                StartupUri = "google.com",
                Role = "Admin",
            };
            int usersCount = await _dbContext.Users.CountAsync();
            usersCount.Should().Be(0);

            // act
            Func<Task> act = async () => { await _service.UpdateUserAsync(user); };

            // verify
            act.Should().Throw<InvalidOperationException>();
            int newUsersCount = await _dbContext.Users.CountAsync();
            newUsersCount.Should().Be(0);

        }

        [Test]
        public async Task GetPasswordForUser_NotChanged_originalPassword()
        {
            // change user without new password, verify its untouched
            var originalPassword = "originalPassword";
            var user = new User()
            {
                Id = 1,
                StartupUri = "google.com",
                Role = "Admin",
                Password = "originalPassword"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            int usersCount = await _dbContext.Users.CountAsync();
            usersCount.Should().Be(1);

            // act
            user.Password = "";
            var result = await _service.GetPasswordForUser(user);

            // verify
            result.Should().Be(originalPassword);
        }

        [Test]
        public async Task GetPasswordForUser_Changed_newHash()
        {
            // setup
            var user = new User()
            {
                Id = 1,
                StartupUri = "google.com",
                Role = "Admin",
                Password = "originalPassword"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            int usersCount = await _dbContext.Users.CountAsync();
            usersCount.Should().Be(1);

            // act
            user.Password = "Changed";
            var result = await _service.GetPasswordForUser(user);

            // verify its changed to hashed by password service
            result.Should().Be(passwordService_passToReturn);
        }

        [Test]
        public async Task AddUserAsync_newUser()
        {
            // setup
            int usersCount = await _dbContext.Users.CountAsync();
            usersCount.Should().Be(0);

            // act
            var user = new User()
            {
                Name = "UserName",
                Password = "Changed",
                StartupUri = "google.com",
                Role = "Admin",
            };
            var result = await _service.AddUserAsync(user);

            // verify its changed to hashed by password service
            int newUsersCount = await _dbContext.Users.CountAsync();
            newUsersCount.Should().Be(1);
            result.Name.Should().Be(user.Name);
            result.Password.Should().Be(user.Password);
            result.StartupUri.Should().Be(user.StartupUri);
            result.Role.Should().Be(user.Role);
        }

        [Test]
        public async Task AddUserAsync_duplicatedName_DbUpdateException()
        {
            // setup
            var user = new User()
            {
                Name = "UserName",
                Password = "Changed",
                StartupUri = "google.com",
                Role = "Admin",
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            int usersCount = await _dbContext.Users.CountAsync();
            usersCount.Should().Be(1);

            // act
            Func<Task> act = async () => { await _service.AddUserAsync(user); };

            // verify
            act.Should().Throw<DbUpdateException>();
            // verify its not added
            int newUsersCount = await _dbContext.Users.CountAsync();
            newUsersCount.Should().Be(1);
        }

        [Test]
        public async Task DeleteUserAsync_true()
        {
            var id = 1;
            _dbContext.Users.Add(new User()
            {
                Id = id
            });
            await _dbContext.SaveChangesAsync();
            int usersCount = await _dbContext.Users.CountAsync();
            usersCount.Should().Be(1);

            // act
            var result = await _service.DeleteUserAsync(id);

            // verify
            result.Should().BeTrue();
            int newUsersCount = await _dbContext.Users.CountAsync();
            newUsersCount.Should().Be(0);
        }

        // TODO
        // add user when user with such name already exist, should throw error
    }
}
