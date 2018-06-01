using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class UserService
    {
        private MainDbContext _context;

        public UserService(MainDbContext dbContextFunc)
        {
            _context = dbContextFunc;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Heroes;
        }

        public Task<User> GetUserAsync(int id)
        {
            return _context.Heroes.SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

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
                _context.Heroes.Add(user);
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
            var user = await _context.Heroes.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return false;
            }

            _context.Heroes.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool UserExists(int id)
        {
            return _context.Heroes.Any(e => e.Id == id);
        }
    }
}
 