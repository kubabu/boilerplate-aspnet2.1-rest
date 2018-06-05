using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Employee
    {
        // example - relations in EF
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //1 to many relation between Employee and Shop
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}
