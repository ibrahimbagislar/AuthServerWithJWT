namespace AuthServer.Core.Entities
{
    public class UserRefreshToken
    {
        public string UserId { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
