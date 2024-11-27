using OAuthExample.Service.Enums;
using OAuthExample.Service.Models;

namespace OAuthExample.Service
{
    public interface IOAuthService
    {
        /// <summary> 驗證方法 </summary>
        AuthenticationMethodEnum AuthenticationMethod { get; }

        /// <summary> 取得登入頁面網址 </summary>
        string GetLoginPageUrl(string state);

        /// <summary> 登入 </summary>
        Task<LoginDataDto> Login(string code);
    }
}