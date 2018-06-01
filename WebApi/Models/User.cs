using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string QrIdentifier { get; set; }
        public string StartupUri { get; set; }

        ////1 to many relation between Employee and Shop
        //public int ShopId { get; set; }
        //public Shop Shop { get; set; }
    }
}
