using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnixWebApi.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // many to 1 relation between shop and employees
        public ICollection<Employee> Employees { get; set; }

        // many to many relation with shops
        public ICollection<ShopProduct> ShopProducts { get; set; }
    }
}
