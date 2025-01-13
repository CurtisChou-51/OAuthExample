namespace OAuthExample.Service.Services
{
    public interface IStateManageService
    {
        /// <summary> 產生一個 state 字串用於 OAuth 的 CSRF 驗證 </summary>
        string GenerateState();

        /// <summary> 驗證 state </summary>
        bool ValidateState(string state);
    }
}