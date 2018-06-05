using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface ICheckPasswordService
    {
        string HashPassword(string password);
        bool IsValidPassword(string userSubmited, string hashed);
        bool IsPasswordValidForUser(User user, string password);
    }
}
