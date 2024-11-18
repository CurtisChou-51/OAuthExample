using System.Security.Claims;
using OAuthExample.Service.Enums;

namespace OAuthExample.Service
{
    public interface IOAuthService
    {
        /// <summary> 驗證方法 </summary>
        AuthenticationMethodEnum AuthenticationMethod { get; }

        /// <summary> 取得登入頁面網址 </summary>
        string GetLoginPageUrl();

        /// <summary> 登入 </summary>
        Task<List<Claim>> Login(string code);
    }
}