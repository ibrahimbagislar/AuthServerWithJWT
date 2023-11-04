using AuthServer.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.DataAccess.Contexts
{
    public class AuthServerAppDbContext : IdentityDbContext<AppUser,IdentityRole,string>
    {
        public AuthServerAppDbContext(DbContextOptions<AuthServerAppDbContext> options) : base(options)
        {
            
        }
        public DbSet<UserRefreshToken>? UserRefreshTokens { get; set; }
        public DbSet<Product>? Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(builder);
        }
    }
}
