namespace SocialNetworkAppLibrary.Data.Models;
public class UserFollow
{
    public string FollowingId { get; set; }
    public string FollowerId { get; set; }

    public ApiUser Following { get; set; } = null!;
    public ApiUser Follower { get; set; } = null!;
}
