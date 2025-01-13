using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OAuthExample.Service.Enums;
using OAuthExample.Service.Models;
using OAuthExample.Service.Options;
using System.Net.Http.Headers;

namespace OAuthExample.Service.Clients
{
    public class GithubOAuthService : IOAuthService
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GithubLoginOptions _options;

        public GithubOAuthService(ILogger<GithubOAuthService> logger, IHttpClientFactory httpClientFactory, IOptions<GithubLoginOptions> options)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public AuthenticationMethodEnum AuthenticationMethod => AuthenticationMethodEnum.Github;

        public string GetLoginPageUrl(string state)
        {
            return $"{_options.EndPoint.Authorize}?scope=user&state={state}&response_type=code&redirect_uri={_options.CallbackUrl}&client_id={_options.ClientId}";
        }

        public async Task<LoginClientDataDto> Login(string code)
        {
            string tokenJsonStr = await GetToken(code);
            TokenDto tokenDto = JsonConvert.DeserializeObject<TokenDto>(tokenJsonStr) ?? new();
            string userInfoJson = await GetUserInfo(tokenDto.access_token);
            UserInfoDto userInfoDto = JsonConvert.DeserializeObject<UserInfoDto>(userInfoJson) ?? new();
            return new LoginClientDataDto
            {
                Id = userInfoDto.id,
                Name = userInfoDto.name,
                AuthenticationMethod = AuthenticationMethod
            };
        }

        private async Task<string> GetToken(string code)
        {
            var client = _httpClientFactory.CreateClient();
            var req = new HttpRequestMessage(HttpMethod.Post, _options.EndPoint.Token);
            req.Headers.Add("Accept", "application/json");
            req.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", _options.CallbackUrl),
                new KeyValuePair<string, string>("client_id", _options.ClientId),
                new KeyValuePair<string, string>("client_secret", _options.ClientSecret)
            });
            var resp = await client.SendAsync(req);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        private async Task<string> GetUserInfo(string token)
        {
            var client = _httpClientFactory.CreateClient();
            var req = new HttpRequestMessage(HttpMethod.Get, _options.EndPoint.UserInfo);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Headers.Add("User-Agent", _options.AppName);
            var resp = await client.SendAsync(req);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        private class TokenDto
        {
            public string access_token { get; set; } = string.Empty;
            public string scope { get; set; } = string.Empty;
            public string token_type { get; set; } = string.Empty;
        }

        private class UserInfoDto
        {
            public string id { get; set; } = string.Empty;
            public string login { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
            public string type { get; set; } = string.Empty;
        }
    }
}
