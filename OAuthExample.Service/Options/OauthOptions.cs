namespace OAuthExample.Service.Options
{
    public class EndPointOptions
    {
        public string Authorize { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string UserInfo { get; set; } = string.Empty;
    }

    public class LineLoginOptions
    {
        public EndPointOptions EndPoint { get; set; } = new();
        public string CallbackUrl { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }

    public class GoogleLoginOptions
    {
        public EndPointOptions EndPoint { get; set; } = new();
        public string CallbackUrl { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }

    public class GithubLoginOptions
    {
        public EndPointOptions EndPoint { get; set; } = new();
        public string CallbackUrl { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string AppName { get; set; } = string.Empty;
    }

    public class MicrosoftLoginOptions
    {
        public EndPointOptions EndPoint { get; set; } = new();
        public string CallbackUrl { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
