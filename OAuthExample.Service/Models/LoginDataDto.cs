using OAuthExample.Service.Enums;

namespace OAuthExample.Service.Models
{
    public class LoginDataDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public AuthenticationMethodEnum AuthenticationMethod { get; set; }
    }
}
