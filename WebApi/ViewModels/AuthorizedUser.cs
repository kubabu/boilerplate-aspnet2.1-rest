namespace WebApi.Models
{
    public class AuthorizedUser: IAppUser
    {
        // mask password and primary key
        public string Name { get; set; }
        public string QrIdentifier { get; set; }
        public string StartupUri { get; set; }
        public string Role { get; set; }
        
        public AuthorizedUser(IAppUser user)
        {
            Name = user.Name;
            QrIdentifier = user.QrIdentifier;
            StartupUri = user.StartupUri;
            Role = user.Role;
        }
    }
}
