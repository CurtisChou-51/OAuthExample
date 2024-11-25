using OAuthExample.Service.Models;
using OAuthExample.Web.Models;

namespace OAuthExample.Web.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        /// <summary> 取得或建立使用者資訊 </summary>
        public LoginUserInfoDto GetOrCreateUserInfo(LoginDataDto loginData)
        {
            // 此範例沒有實際資料庫，模擬由 Repository 取得或建立使用者資訊
            return new LoginUserInfoDto
            {
                UserId = 1,
                UserName = loginData.Name,
                AuthenticationMethod = loginData.AuthenticationMethod.ToString()
            };
        }
    }
}
