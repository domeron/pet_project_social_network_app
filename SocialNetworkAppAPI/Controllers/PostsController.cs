
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkAppAPI.Services;
using SocialNetworkAppLibrary.Data.DTO;
using SocialNetworkAppLibrary.Data.Models;
using SocialNetworkAppLibrary.Data.ViewModels;
using SocialNetworkAppLibrary.Exceptions;

namespace SocialNetworkAppAPI.Controllers
{
    [Route("posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IPostsService _postsService;

        public PostsController(IPostsService postsService, ILogger<PostsController> logger)
        {
            _postsService = postsService;
            _logger = logger;
        }

        [HttpGet]
        [Route("all")]
        public async IAsyncEnumerable<PostViewModel> GetPosts()
        {
            _logger.LogInformation("GetPosts method started");

            var posts = _postsService.GetPosts();
            await foreach (var post in posts)
            {
                yield return new PostViewModel(post);
            }
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async IAsyncEnumerable<PostViewModel> GetUserPosts(string userId)
        {
            _logger.LogInformation("GetUserPosts method started");

            var posts = _postsService.GetUserPosts(userId);
            await foreach (var post in posts)
            {
                yield return new PostViewModel(post);
            }
        }

        [HttpPost]
        [Route("create")]
        [Authorize]
        public async Task<IActionResult> CreatePostAsync(PostCreateDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _logger.LogInformation("CreatePost method started");

            (Post? post, Exception? ex) = await _postsService.CreatePost(model);
            if (post != null && ex == null)
                return Created(nameof(CreatePostAsync), post.Id);
            else if (ex is NotFoundException)
                return NotFound();
            else return StatusCode(500, ex);
        }


        [HttpPost]
        [Route("update")]
        [Authorize]
        public async Task<IActionResult> UpdatePostAsync(PostUpdateDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _logger.LogInformation("UpdatePost method started");

            (Post? post, Exception? ex) = await _postsService.UpdatePost(model);
            if (post != null && ex == null)
                return Ok(post);
            else if (ex is NotFoundException)
                return NotFound();
            else return StatusCode(500, ex);
        }

        [HttpDelete]
        [Route("delete/{postId:int}")]
        [Authorize]
        public async Task<IActionResult> DeletePostAsync(int postId)
        {
            _logger.LogInformation("DeletePost method started");

            (Post? post, Exception? ex) = await _postsService.DeletePost(postId);
            if (post != null && ex == null)
                return Ok(post);
            else if (ex is NotFoundException)
                return NotFound();
            else return StatusCode(500, ex);
        }

    }
}
