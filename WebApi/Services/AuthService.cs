using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.DbContexts;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class AuthService: IAuthorizeService
    {
        private MainDbContext _context;

        public AuthService(MainDbContext dbContext)
        {
            _context = dbContext;
        }

        public User AuthorizeWithLoginAndPassword(string login, string password)
        {
            //throw new NotImplementedException();
            var user = _context.Users.SingleOrDefault(m => m.Name == login);


            //if()

            return user;
        }

        public async Task<User> AuthorizeWithLoginAndPasswordAsync(string login, string password)
        {
            //throw new NotImplementedException();
            var user = await _context.Users.FirstAsync(); // SingleOrDefaultAsync(m => m.Name == login);
            //if(request.Username == "foo" && request.Password == "bar") TODO
            //if()

            return user;
        }
    }
}
