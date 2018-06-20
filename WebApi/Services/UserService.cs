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
            _context.Entry(user).State = EntityState.Modified;
            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrWhiteSpace(user.Password))
            {
                var userCurrentState = await GetUserAsync(user.Id); // keep current password
                user.Password = userCurrentState.Password;
            }
            else
            {
                user.Password = _checkPasswordService.HashPassword(user.Password);
            }

            try
            {
                await _context.SaveChangesAsync();
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
            }
            return true;
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
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
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
    }
}
 