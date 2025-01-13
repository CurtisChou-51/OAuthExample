namespace OAuthExample.Service.Models
{
    public class OAuthLoginUrlDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string Url { get; set; } = string.Empty;
    }

    public class LoginResultDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public LoginUserInfoDto? UserInfo { get; set; }
    }

    public class LoginUserInfoDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string AuthenticationMethod { get; set; } = string.Empty;
    }
}
