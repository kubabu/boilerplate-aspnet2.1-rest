using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repositories.Interfaces
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
