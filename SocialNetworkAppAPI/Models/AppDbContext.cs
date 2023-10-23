using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetworkAppLibrary.Models;

namespace SocialNetworkAppAPI.Models
{
    public class AppDbContext : IdentityDbContext<ApiUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Post> Posts => Set<Post>();
    }
}
