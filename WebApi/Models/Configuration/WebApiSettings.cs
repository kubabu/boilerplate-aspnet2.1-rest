using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Configuration
{
    public class JwtSettings
    {
        public string JwtKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }

    public class WebApiSettings
    {
        public List<string> CorsClientUrls { get; set; }
        public JwtSettings JwtSettings { get; set; }
    }
}
