using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebApi.Models.DbContexts.Interfaces;

namespace WebApi.Models.DbContexts
{
    public class MainDbContext : DbContext, IAuthDbContext
    {
        public DbSet<User> Users { get; set; }
    
        //public DbSet<Employee> Employees { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<Shop> Shops { get; set; }
        // public DbSet<ShopProduct> ShopProducts { get; set; }


        public MainDbContext(DbContextOptions<MainDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(h => h.Id);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();
            // example - relations in EF
            //modelBuilder.Entity<ShopProduct>()
            //            .HasKey(entity => new { entity.ProductId, entity.ShopId });

            //modelBuilder.Entity<ShopProduct>()
            //            .HasOne(sp => sp.Product)
            //            .WithMany(p => p.ProductShops)
            //            .HasForeignKey(sp => sp.ProductId);

            //modelBuilder.Entity<ShopProduct>()
            //             .HasOne(sp => sp.Shop)
            //             .WithMany(s => s.ShopProducts)
            //             .HasForeignKey(sp => sp.ShopId);
        }

        async Task<int> IWebApiDbContext.SaveChangesAsync()
        {
            return await SaveChangesAsync();
        }
    }
}
