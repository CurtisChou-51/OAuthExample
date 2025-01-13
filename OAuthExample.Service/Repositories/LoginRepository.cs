using OAuthExample.Service.Entities;

namespace OAuthExample.Service.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        /// <summary> 取得或建立使用者資訊 </summary>
        public UserInfoEntity GetOrCreateUserInfo(UserLoginLinkEntity userLoginLinkEntity, UserInfoEntity userInfoEntity)
        {
            // 在實際應用中，這裡應該包含資料庫存取邏輯
            // 例如：檢查使用者是否存在於資料庫，如果不存在則建立新使用者
            // get user by userLoginLinkEntity
            // 此範例沒有實際使用資料庫，簡單模擬從資料庫取得或建立使用者資訊
            return new UserInfoEntity
            {
                UserId = 1, // 假設從資料庫取得的使用者ID
                UserName = userInfoEntity.UserName,
                Bio = "foo bar"
            };
        }
    }
}
