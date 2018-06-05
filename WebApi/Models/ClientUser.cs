using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class ClientUser
    {
        // mask password, primary key
        public string Name { get; set; }
        public string Description { get; set; }
        public string QrIdentifier { get; set; }
        public string StartupUri { get; set; }

        public ClientUser(User user)
        {
            Name = user.Name;
            Description = user.Description;
            QrIdentifier = user.QrIdentifier;
            StartupUri = user.StartupUri;
        }
    }
}
