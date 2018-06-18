using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Configuration
{
    public class WebApiSettings
    {
        public List<string> CorsClientUrls { get; set; }
        public JwtSettings JwtSettings { get; set; }
        //public string CompletationOrdersHubUrl { get => "/Hubs/CompletationOrders"; }
    }
}
