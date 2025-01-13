using Microsoft.Extensions.Options;
using OAuthExample.Web.Helpers;
using OAuthExample.Web.Options;

namespace OAuthExample.Web.Services
{
    /// <summary> 此實作的 state 使用加解密來進行處理，也可以使用 session 或其他機制來管理 state </summary>
    public class StateManageService : IStateManageService
    {
        private readonly TimeSpan _validityDuration;
        private readonly TimeProvider _timeProvider;
        private readonly string _encryptionKey;

        public StateManageService(IOptions<StateManageOptions> options, TimeProvider timeProvider)
        {
            _validityDuration = TimeSpan.FromMinutes(options.Value.ValidityMinutes);
            _encryptionKey = options.Value.EncryptionKey;
            _timeProvider = timeProvider;
        }

        /// <summary> 產生一個 state 字串用於 OAuth 的 CSRF 驗證 </summary>
        public string GenerateState()
        {
            // 建立時間戳 > 加密 > state
            string rawTimestamp = _timeProvider.GetUtcNow().ToUnixTimeSeconds().ToString();
            return EncryptHelper.Encrypt(rawTimestamp, _encryptionKey);
        }

        /// <summary> 驗證 state </summary>
        public bool ValidateState(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                return false;
            try
            {
                // state > 解密 > 驗證時間戳
                string rawTimestamp = EncryptHelper.Decrypt(state, _encryptionKey);
                if (!long.TryParse(rawTimestamp, out long timestamp))
                    return false;

                DateTimeOffset stateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp);
                return _timeProvider.GetUtcNow() - stateTime <= _validityDuration;
            }
            catch
            {
                return false;
            }
        }
    }
}
