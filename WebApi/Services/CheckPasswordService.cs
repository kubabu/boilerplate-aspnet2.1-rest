using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class CheckPasswordService : ICheckPasswordService
    {
        public string HashPassword(string clearTextPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(clearTextPassword);
        }

        public bool IsValidPassword(string userSubmitted, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(userSubmitted, hashed);
        }
    }
}
