﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class AuthorizedUser: IAppUser
    {
        // mask password, primary key
        public string Name { get; set; }
        public string Description { get; set; }
        public string QrIdentifier { get; set; }
        public string StartupUri { get; set; }
        public string Role { get; set; }
        
        public AuthorizedUser(IAppUser user)
        {
            Name = user.Name;
            //Description = user.Description;
            QrIdentifier = user.QrIdentifier;
            StartupUri = user.StartupUri;
            Role = "Admin";
        }
    }
}
