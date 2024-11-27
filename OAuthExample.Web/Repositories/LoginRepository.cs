using OAuthExample.Service.Models;
using OAuthExample.Web.Models;

namespace OAuthExample.Web.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        /// <summary> 取得或建立使用者資訊 </summary>
        public LoginUserInfoDto GetOrCreateUserInfo(LoginDataDto loginData)
        {
            // 在實際應用中，這裡應該包含資料庫存取邏輯
            // 例如：檢查使用者是否存在於資料庫，如果不存在則建立新使用者

            // 此範例沒有實際使用資料庫，簡單模擬從資料庫取得或建立使用者資訊
            var userInfo = new LoginUserInfoDto
            {
                UserId = 1, // 假設從資料庫取得的使用者ID
                UserName = loginData.Name,
                AuthenticationMethod = loginData.AuthenticationMethod.ToString()
            };
            return userInfo;
        }
    }
}
