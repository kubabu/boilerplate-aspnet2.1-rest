using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UserViewModel
    {
        // mask password, primary key
        public string Name { get; set; }
        public string Description { get; set; }
        public string QrIdentifier { get; set; }
        public string StartupUri { get; set; }
        public string Role { get; set; }
        public IEnumerable<string> Tabs { get; set; }

        public UserViewModel(User user)
        {
            Name = user.Name;
            Description = user.Description;
            QrIdentifier = user.QrIdentifier;
            StartupUri = user.StartupUri;
            Role = "Admin";
        }
    }
}
