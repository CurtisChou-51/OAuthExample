namespace OAuthExample.Service.Entities
{
    public class UserLoginLinkEntity
    {
        public string ClientId { get; set; } = string.Empty;
        public long UserId { get; set; }
        public string AuthenticationMethod { get; set; } = string.Empty;
    }
}
