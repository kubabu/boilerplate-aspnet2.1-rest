using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class CheckPasswordService : ICheckPasswordService
    {
        public string HashPassword(string clearTextPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(clearTextPassword);
        }

        public bool IsPasswordValidForUser(User user, string password)
        {
            return IsValidPassword(password, user.Password);
        }

        public bool IsValidPassword(string userSubmitted, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(userSubmitted, hashed);
        }
    }
}
