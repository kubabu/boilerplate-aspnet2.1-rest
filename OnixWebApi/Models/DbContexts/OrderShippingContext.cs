using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnixWebApi.Models.DbContexts
{
    public class OrderShippingContext : DbContext
    {
        public OrderShippingContext(DbContextOptions<OrderShippingContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hero>()
              .HasKey(h => h.Id);
        }

        public DbSet<Hero> Heroes { get; set; }
    }
}
