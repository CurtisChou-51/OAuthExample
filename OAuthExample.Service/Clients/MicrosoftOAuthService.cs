using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OAuthExample.Service.Enums;
using OAuthExample.Service.Models;
using OAuthExample.Service.Options;
using System.Net.Http.Headers;

namespace OAuthExample.Service.Clients
{
    public class MicrosoftOAuthService : IOAuthService
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MicrosoftLoginOptions _options;

        public MicrosoftOAuthService(ILogger<MicrosoftOAuthService> logger, IHttpClientFactory httpClientFactory, IOptions<MicrosoftLoginOptions> options)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public AuthenticationMethodEnum AuthenticationMethod => AuthenticationMethodEnum.Microsoft;

        public string GetLoginPageUrl(string state)
        {
            return $"{_options.EndPoint.Authorize}?scope=user.read+openid+profile+email&&state={state}&response_type=code&redirect_uri={_options.CallbackUrl}&client_id={_options.ClientId}";
        }

        public async Task<LoginClientDataDto> Login(string code)
        {
            string tokenJsonStr = await GetToken(code);
            TokenDto tokenDto = JsonConvert.DeserializeObject<TokenDto>(tokenJsonStr) ?? new();
            string userInfoJson = await GetUserInfo(tokenDto.access_token);
            UserInfoDto userInfoDto = JsonConvert.DeserializeObject<UserInfoDto>(userInfoJson) ?? new();
            string displayName = userInfoDto.name;
            if (string.IsNullOrWhiteSpace(displayName))
                displayName = $"{userInfoDto.givenname} {userInfoDto.familyname}";
            return new LoginClientDataDto
            {
                Id = userInfoDto.sub,
                Name = displayName,
                AuthenticationMethod = AuthenticationMethod
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
            public string token_type { get; set; } = string.Empty;
            public int expires_in { get; set; }
            public string scope { get; set; } = string.Empty;
        }

        private class UserInfoDto
        {
            public string sub { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
            public string givenname { get; set; } = string.Empty;
            public string familyname { get; set; } = string.Empty;
            public string picture { get; set; } = string.Empty;
            public string email { get; set; } = string.Empty;
        }
    }
}
