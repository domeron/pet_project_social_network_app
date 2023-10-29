using Microsoft.EntityFrameworkCore;
using SocialNetworkAppAPI.Data;
using SocialNetworkAppLibrary.Data.Models;
using SocialNetworkAppLibrary.Exceptions;

namespace SocialNetworkAppAPI.Repositories;

public interface IUserRepository {
    Task<ApiUser?> GetUserAsync(string userId);
    IAsyncEnumerable<ApiUser> GetUsersByUsername(string username);
    IAsyncEnumerable<ApiUser> GetUserFollowers(ApiUser user);
    IAsyncEnumerable<ApiUser> GetUserFollowings(ApiUser user);
    Task<bool> IsFollowing(string followerId, string followingId);

    Task FollowUser(string followerId, string followingId);
    Task UnfollowUser(string followerId, string followingId);
    Task DeleteFromFollowers(string followerId, string followingId);
}
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    { 
        _context = context;
    }

    public async Task<ApiUser?> GetUserAsync(string userId)
    {
        return await _context.Users.Where(u => u.Id.Equals(userId)).FirstOrDefaultAsync();
    }

    public async IAsyncEnumerable<ApiUser> GetUsersByUsername(string username)
    {
        var users = _context.Users.Where(u => u.UserName!.StartsWith(username)).AsAsyncEnumerable();

        await foreach (var user in users) {
            yield return user;
        }
    }

    public async IAsyncEnumerable<ApiUser> GetUserFollowers(ApiUser user)
    {
        var followers = _context.UserFollows.Select(uf => uf.Follower).AsAsyncEnumerable();

        await foreach (var follower in followers) { 
            yield return follower;
        }
    }

    public async IAsyncEnumerable<ApiUser> GetUserFollowings(ApiUser user)
    {
        var followings = _context.UserFollows.Select(uf => uf.Following).AsAsyncEnumerable();

        await foreach(var following in followings) { yield return following; }
    }

    public async Task<bool> IsFollowing(string followerId, string followingId)
    {
        return await _context.UserFollows.Where(uf => uf.FollowerId.Equals(followerId) 
            && uf.FollowingId.Equals(followingId)).AnyAsync();
    }

    public async Task FollowUser(string followerId, string followingId)
    {
        var user1 = await GetUserAsync(followerId) ?? throw new UserNotFoundException();
        var user2 = await GetUserAsync(followingId) ?? throw new UserNotFoundException();

        if(await IsFollowing(followerId, followingId)) throw new AlreadyFollowedException();

        user1!.Followings.Add(new UserFollow
        {
            FollowerId = followerId,
            FollowingId = followingId
        });
        user1.FollowingsCount++;
        user2.FollowersCount++;

        await _context.SaveChangesAsync();
    }

    public async Task UnfollowUser(string followerId, string followingId)
    {
        var follower = await GetUserAsync(followerId) ?? throw new UserNotFoundException();
        var following = await GetUserAsync(followingId) ?? throw new UserNotFoundException();

        var userFollow = await _context.UserFollows.Where(uf => uf.FollowerId.Equals(followerId)
            && uf.FollowingId.Equals(followingId))
            .FirstOrDefaultAsync() ?? throw new NotFollowedException();

        follower.Followings.Remove(userFollow);
        follower.FollowingsCount--;
        following.FollowersCount--;

        _context.Entry(userFollow).State = EntityState.Deleted;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFromFollowers(string followerId, string followingId)
    {
        var follower = await GetUserAsync(followerId) ?? throw new UserNotFoundException();
        var following = await GetUserAsync(followingId) ?? throw new UserNotFoundException();

        var userFollow = await _context.UserFollows.Where(uf => uf.FollowerId.Equals(followerId)
            && uf.FollowingId.Equals(followingId))
            .FirstOrDefaultAsync() ?? throw new NotFollowedException();

        following.Followers.Remove(userFollow);
        follower.FollowingsCount--;
        following.FollowersCount--;

        _context.Entry(userFollow).State = EntityState.Deleted;
        await _context.SaveChangesAsync();
    }
}
