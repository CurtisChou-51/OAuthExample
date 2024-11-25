using OAuthExample.Service.Models;
using OAuthExample.Web.Models;

namespace OAuthExample.Web.Repositories
{
    public interface ILoginRepository
    {
        /// <summary> 取得或建立使用者資訊 </summary>
        LoginUserInfoDto GetOrCreateUserInfo(LoginDataDto loginData);
    }
}