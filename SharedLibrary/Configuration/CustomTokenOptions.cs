namespace SharedLibrary.Configuration
{
    public class CustomTokenOptions
    {
        public List<string>? Audience { get; set; }
        public string? Issuer { get; set; }
        public int AccessTokenExpirationMinute { get; set; }
        public int RefreshTokenExpirationMinute { get; set; }
        public string? SecurityKey { get; set; }
    }

}
