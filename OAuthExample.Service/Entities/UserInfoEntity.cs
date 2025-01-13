namespace OAuthExample.Service.Entities
{
    public class UserInfoEntity
    {
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;

        // 其他另外功能維護的資料
        public DateTime CreateTime { get; set; }
        public string? Bio { get; set; }
    }
}
