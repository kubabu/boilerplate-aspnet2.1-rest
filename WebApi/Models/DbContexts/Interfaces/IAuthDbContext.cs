using Microsoft.EntityFrameworkCore;

namespace WebApi.Models.DbContexts.Interfaces
{
    public interface IAuthDbContext: IWebApiDbContext
    {
        DbSet<User> Users { get; set; }
    }
}
