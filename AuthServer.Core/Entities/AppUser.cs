using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string? City { get; set; }
    }
}
