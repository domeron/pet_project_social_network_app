using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkAppAPI.Services;
using SocialNetworkAppLibrary.Data.DTO;
using SocialNetworkAppLibrary.Data.Models;
using SocialNetworkAppLibrary.Data.ViewModels;
using SocialNetworkAppLibrary.Exceptions;

namespace SocialNetworkAppAPI.Controllers;

[ApiController]
[Route("user")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    { 
        _userService = userService;
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var user = await _userService.GetUserById(userId);
        if (user == null) return NotFound();

        return Ok(new UserViewModel(user));
    }

    [HttpGet]
    [Route("search/username/{pattern}")]
    public async IAsyncEnumerable<UserViewModel> GetUsersByUsername(string pattern)
    { 
        var users = _userService.GetUsersByUsername(pattern);

        await foreach (var user in users)
        {
            yield return new UserViewModel(user);
        }
    }

    [HttpGet]
    [Route("isFollowing")]
    public async Task<IActionResult> CheckIfFollowing(string followerId, string followingId)
    { 
        (bool isFollowing, Exception? ex) = await _userService.IsFollowing(followerId, followingId);

        if (ex == null) return Ok(isFollowing);
        else if (ex is UserNotFoundException) return NotFound();
        else return StatusCode(500);
    }

    [HttpGet]
    [Route("{userId}/followers")]
    public async IAsyncEnumerable<UserViewModel> GetUserFollowers(string userId)
    {
        var followers = _userService.GetUserFollowers(userId);
        await foreach (var follower in followers)
        {
            yield return new UserViewModel(follower);
        }
    }

    [HttpGet]
    [Route("{userId}/followings")]
    public async IAsyncEnumerable<UserViewModel> GetUserFollowings(string userId)
    {
        var followings = _userService.GetUserFollowings(userId);

        await foreach (var following in followings) { yield return new UserViewModel(following); }
    }

    [HttpPost]
    [Authorize]
    [Route("follow")]
    public async Task<IActionResult> Follow(FollowDTO model)
    { 
        if(!ModelState.IsValid) return BadRequest();

        (bool followed, Exception? ex) = await _userService.FollowUser(model.FollowerId, model.FollowingId);
        if (ex == null) return Ok();
        else if (ex is UserNotFoundException) return NotFound();
        else if (ex is AlreadyFollowedException) return BadRequest();
        else return StatusCode(500);
    }

    [HttpPost]
    [Authorize]
    [Route("unfollow")]
    public async Task<IActionResult> Unfollow(FollowDTO model)
    {
        if (!ModelState.IsValid) return BadRequest();

        (bool unfollowed, Exception? ex) = await _userService.UnfollowUser(model.FollowerId, model.FollowingId);
        if (ex == null) return Ok();
        else if (ex is UserNotFoundException) return NotFound();
        else if (ex is NotFollowedException) return BadRequest();
        else return StatusCode(500);
    }

    [HttpPost]
    [Authorize]
    [Route("delete-follower")]
    public async Task<IActionResult> DeleteFollower(FollowDTO model)
    {
        if (!ModelState.IsValid) return BadRequest();

        (bool unfollowed, Exception? ex) = await _userService.DeleteFromFollowers(model.FollowerId, model.FollowingId);
        if (ex == null) return Ok();
        else if (ex is UserNotFoundException) return NotFound();
        else if (ex is NotFollowedException) return BadRequest();
        else return StatusCode(500);
    }
}
