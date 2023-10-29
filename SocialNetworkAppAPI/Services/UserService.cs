using SocialNetworkAppAPI.Repositories;
using SocialNetworkAppLibrary.Data.Models;
using SocialNetworkAppLibrary.Exceptions;

namespace SocialNetworkAppAPI.Services;

public interface IUserService
{
    Task<ApiUser?> GetUserById(string userId);
    IAsyncEnumerable<ApiUser> GetUsersByUsername(string username);
    IAsyncEnumerable<ApiUser> GetUserFollowers(string userId);
    IAsyncEnumerable<ApiUser> GetUserFollowings(string userId);
    Task<(bool, Exception?)> IsFollowing(string followerId, string followeeId);

    Task<(bool, Exception?)> FollowUser(string followerId, string followingId);
    Task<(bool, Exception?)> UnfollowUser(string followerId, string followingId);
    Task<(bool, Exception?)> DeleteFromFollowers(string followerId, string followingId);
}
public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepository)
    { 
        _userRepo = userRepository;
    }

    public async Task<ApiUser?> GetUserById(string userId)
    {
        return await _userRepo.GetUserAsync(userId);
    }

    public async IAsyncEnumerable<ApiUser> GetUsersByUsername(string username) { 
        var users = _userRepo.GetUsersByUsername(username);

        await foreach (var user in users) {
            yield return user;
        }
    }

    public async IAsyncEnumerable<ApiUser> GetUserFollowers(string userId)
    {
        var user = await _userRepo.GetUserAsync(userId) ?? throw new UserNotFoundException();
        var followers = _userRepo.GetUserFollowers(user);
        await foreach (var follower in followers) { 
            yield return follower;
        } 
    }

    public async IAsyncEnumerable<ApiUser> GetUserFollowings(string userId)
    {
        var user = await _userRepo.GetUserAsync(userId) ?? throw new UserNotFoundException();
        var followings = _userRepo.GetUserFollowings(user);

        await foreach(var following in followings) { yield return following; }
    }

    public async Task<(bool, Exception?)> IsFollowing(string followerId, string followingId)
    {
        if (await _userRepo.GetUserAsync(followerId) == null ||
            await _userRepo.GetUserAsync(followingId) == null)
            return (false, new UserNotFoundException());

        return (await _userRepo.IsFollowing(followerId, followingId), null);
    }

    public async Task<(bool, Exception?)> FollowUser(string followerId, string followingId)
    {
        try
        {
            await _userRepo.FollowUser(followerId, followingId);
            return (true, null);
        }
        catch (UserNotFoundException e) { return (false, e); }
        catch (AlreadyFollowedException e) { return (false, e); }
        catch (Exception e) { return (false, e); }
    }

    public async Task<(bool, Exception?)> UnfollowUser(string followerId, string followingId)
    {
        try
        {
            await _userRepo.UnfollowUser(followerId, followingId);
            return (true, null);
        }
        catch (UserNotFoundException e) { return (false, e); }
        catch (NotFollowedException e) { return (false, e); }
        catch (Exception e) { return (false, e); }
    }

    public async Task<(bool, Exception?)> DeleteFromFollowers(string followerId, string followingId)
    {
        try
        {
            await _userRepo.DeleteFromFollowers(followerId, followingId);
            return (true, null);
        }
        catch (UserNotFoundException e) { return (false, e); }
        catch (NotFollowedException e) { return (false, e); }
        catch (Exception e) { return (false, e); }
    }
}
