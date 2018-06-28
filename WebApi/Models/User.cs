namespace WebApi.Models
{
    public class User: IAppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string QrIdentifier { get; set; }
        public string StartupUri { get; set; }
        public string Role { get; set; }
    }
}
