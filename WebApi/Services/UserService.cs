using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.DbContexts;
using WebApi.Services.Interfaces;


namespace WebApi.Services
{
    public class UserService: IServeUsers
    {
        private MainDbContext _context;
        private ICheckPasswordService _checkPasswordService;
        private ILogger<UserService> _logger;

        public UserService(MainDbContext dbContext, ICheckPasswordService checkPasswordService, ILogger<UserService> logger)
        {
            _checkPasswordService = checkPasswordService;
            _context = dbContext;
            _logger = logger;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }

        public Task<User> GetUserAsync(int id)
        {
            return _context.Users.SingleOrDefaultAsync(m => m.Id == id); 
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            //_context.Entry(user).State = EntityState.Modified; // this works if there's no logic on update
            var storedUser = await _context.Users.Where(u => u.Id == user.Id).SingleAsync();
            storedUser.Name = user.Name;
            storedUser.Password = await GetPasswordForUser(user);
            storedUser.Role = user.Role;
            storedUser.QrIdentifier = user.QrIdentifier;
            storedUser.StartupUri = user.StartupUri;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            };
        }

        public async Task<User> AddUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await GetUserAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public async Task<string> GetPasswordForUser(User user)
        {
            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrWhiteSpace(user.Password))
            {
                // keep current password
                var userCurrentPass = await _context.Users
                    .Where(u => u.Id == user.Id)
                    .Select(u => u.Password)
                    .SingleOrDefaultAsync();
                    //.AsNoTracking().SingleOrDefaultAsync(m => m.Id == user.Id).Password;
                return userCurrentPass;
            }
            return _checkPasswordService.HashPassword(user.Password);
        }
    }
}
 