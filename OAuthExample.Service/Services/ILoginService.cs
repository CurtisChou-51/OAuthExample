using OAuthExample.Service.Models;

namespace OAuthExample.Service.Services
{
    public interface ILoginService
    {
        /// <summary> 取得 OAuth 登入 Url </summary>
        OAuthLoginUrlDto GetOAuthLoginUrl(string authenticationMethod);

        /// <summary> OAuth 登入 </summary>
        Task<LoginResultDto> OAuthLogin(string authenticationMethod, string code, string state);
    }
}