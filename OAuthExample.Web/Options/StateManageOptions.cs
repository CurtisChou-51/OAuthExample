namespace OAuthExample.Web.Options
{
    public class StateManageOptions
    {
        /// <summary> 有效時間(分鐘) </summary>
        public int ValidityMinutes { get; set; }

        /// <summary> 加密金鑰 </summary>
        public string EncryptionKey { get; set; } = string.Empty;
    }
}
