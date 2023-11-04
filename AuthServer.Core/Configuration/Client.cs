

namespace AuthServer.Core.Configuration
{
    public class Client
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public List<string>? Audiences { get; set; }
    }
}
