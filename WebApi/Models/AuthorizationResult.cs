using System;
namespace WebApi.Models
{
    public class AuthorizationResult
    {
        public AuthorizedUser User { get; set; }
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
