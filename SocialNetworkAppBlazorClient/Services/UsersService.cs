using SocialNetworkAppLibrary.Data.ViewModels;

namespace SocialNetworkAppBlazorClient.Services;

public interface IUserService {
    Task<UserViewModel> GetUserById(string userId);
    IAsyncEnumerable<UserViewModel> GetUsersByUsername(string username);
    IAsyncEnumerable<UserViewModel> GetFriendsOfUser(string userId);
    Task<bool> CheckIfFriends(string firstUserId, string secondUserId);
}
public class UsersService : IUserService 
{
    private readonly IHttpService _httpService;

    public UsersService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<UserViewModel> GetUserById(string userId) 
    {
        var user = await _httpService.Get<UserViewModel>($"/user/{userId}");
        return user;
    }

    public async IAsyncEnumerable<UserViewModel> GetUsersByUsername(string username)
    {
        var users = await _httpService.Get<IAsyncEnumerable<UserViewModel>>($"/user/search/username/{username}");
        await foreach (var user in users) { 
            yield return user;
        }
    }

    public async IAsyncEnumerable<UserViewModel> GetFriendsOfUser(string userId)
    {
        var users = await _httpService.Get<IAsyncEnumerable<UserViewModel>>($"/user/friends/{userId}");
        await foreach (var user in users)
        {
            yield return user;
        }
    }

    public async Task<bool> CheckIfFriends(string firstUserId, string secondUserId)
    {
        return await _httpService.Get<bool>($"/user/areFriends?first={firstUserId}&&second={secondUserId}");
    }
}
