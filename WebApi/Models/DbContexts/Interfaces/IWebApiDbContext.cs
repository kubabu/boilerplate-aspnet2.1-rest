using System.Threading.Tasks;

namespace WebApi.Models.DbContexts.Interfaces
{
    public interface IWebApiDbContext
    {
        Task<int> SaveChangesAsync();
    }
}