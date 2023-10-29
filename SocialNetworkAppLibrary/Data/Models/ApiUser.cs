using Microsoft.AspNetCore.Identity;
using SocialNetworkAppLibrary.Data.Models;

namespace SocialNetworkAppLibrary.Data.Models;

public class ApiUser : IdentityUser
{
    public int FollowersCount { get; set; } = 0;
    public int FollowingsCount { get; set; } = 0;
    public ICollection<Post> Posts { get; set; }
    public ICollection<UserFollow> Followings { get; set; } = new List<UserFollow>();
    public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
}
