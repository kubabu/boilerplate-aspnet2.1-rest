using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IServeUsers
    {
        IEnumerable<User> GetUsers();
        Task<User> GetUserAsync(int id);
        Task<bool> UpdateUserAsync(User user);
        Task<User> AddUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
