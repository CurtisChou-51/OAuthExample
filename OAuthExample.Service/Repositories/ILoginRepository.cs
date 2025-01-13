using OAuthExample.Service.Entities;

namespace OAuthExample.Service.Repositories
{
    public interface ILoginRepository
    {
        /// <summary> 取得或建立使用者資訊 </summary>
        UserInfoEntity GetOrCreateUserInfo(UserLoginLinkEntity userLoginLinkEntity, UserInfoEntity userInfoEntity);
    }
}