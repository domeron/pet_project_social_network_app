using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetworkAppAPI.Data.Configuration;
using SocialNetworkAppLibrary.Data.Models;

namespace SocialNetworkAppAPI.Data;

public class AppDbContext : IdentityDbContext<ApiUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*new PostConfiguration().Configure(modelBuilder.Entity<Post>());*/
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostConfiguration).Assembly);
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<UserFollow> UserFollows => Set<UserFollow>();
}
