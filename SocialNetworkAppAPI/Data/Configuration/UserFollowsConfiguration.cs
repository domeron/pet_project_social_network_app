using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkAppLibrary.Data.Models;

namespace SocialNetworkAppAPI.Data.Configuration;

public class UserFollowsConfiguration : IEntityTypeConfiguration<UserFollow>
{
    public void Configure(EntityTypeBuilder<UserFollow> builder)
    {
        builder.HasKey(uf => new { uf.FollowingId, uf.FollowerId});
        builder.HasOne(uf => uf.Following)
            .WithMany(f => f.Followers)
            .HasForeignKey(uf => uf.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(uf => uf.Follower)
            .WithMany(u => u.Followings)
            .HasForeignKey(uf => uf.FollowerId);
    }
}
