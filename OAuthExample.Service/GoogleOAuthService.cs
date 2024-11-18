using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OAuthExample.Service.Enums;
using OAuthExample.Service.Options;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace OAuthExample.Service
{
    public class GoogleOAuthService : IOAuthService
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GoogleLoginOptions _options;

        public GoogleOAuthService(ILogger<GoogleOAuthService> logger, IHttpClientFactory httpClientFactory, IOptions<GoogleLoginOptions> options)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public AuthenticationMethodEnum AuthenticationMethod => AuthenticationMethodEnum.Google;

        public string GetLoginPageUrl()
        {
            return $"{_options.EndPoint.Authorize}?scope=email profile&include_granted_scopes=true&response_type=code&state=xxxxxxxxxxxxxxxx&redirect_uri={_options.CallbackUrl}&client_id={_options.ClientId}";
        }

        public async Task<List<Claim>> Login(string code)
        {
            string tokenJsonStr = await GetToken(code);
            TokenDto tokenDto = JsonConvert.DeserializeObject<TokenDto>(tokenJsonStr) ?? new();
            string userInfoJson = await GetUserInfo(tokenDto.access_token);
            UserInfoDto userInfoDto = JsonConvert.DeserializeObject<UserInfoDto>(userInfoJson) ?? new();
            return new List<Claim>
            {
                new(ClaimTypes.AuthenticationMethod, AuthenticationMethod.ToString()),
                new(ClaimTypes.Name, userInfoDto.name)
            };
        }

        private async Task<string> GetToken(string code)
        {
            var client = _httpClientFactory.CreateClient();
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", _options.CallbackUrl),
                new KeyValuePair<string, string>("client_id", _options.ClientId),
                new KeyValuePair<string, string>("client_secret", _options.ClientSecret)
            });
            var resp = await client.PostAsync(_options.EndPoint.Token, formData);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        private async Task<string> GetUserInfo(string token)
        {
            var client = _httpClientFactory.CreateClient();
            var req = new HttpRequestMessage(HttpMethod.Get, _options.EndPoint.UserInfo);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resp = await client.SendAsync(req);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        private class TokenDto
        {
            public string access_token { get; set; } = string.Empty;
            public int expires_in { get; set; }
            public string scope { get; set; } = string.Empty;
            public string token_type { get; set; } = string.Empty;
            public string id_token { get; set; } = string.Empty;
        }

        private class UserInfoDto
        {
            public string id { get; set; } = string.Empty;
            public string email { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
            public string picture { get; set; } = string.Empty;
        }
    }
}
