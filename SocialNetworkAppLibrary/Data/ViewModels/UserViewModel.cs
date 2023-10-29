using SocialNetworkAppLibrary.Data.Models;

namespace SocialNetworkAppLibrary.Data.ViewModels;
public class UserViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    public int FollowersCount { get; set; }
    public int FollowingsCount { get; set; }

    public UserViewModel(ApiUser user) {
        UserId = user.Id;
        UserName = user.UserName;
        Email = user.Email;
        FollowersCount = user.FollowersCount;
        FollowingsCount = user.FollowingsCount;
    }

    public UserViewModel() { }
}
