
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialNetworkAppAPI.DTO;
using SocialNetworkAppAPI.Models;
using SocialNetworkAppLibrary.DTO;
using SocialNetworkAppLibrary.Models;

namespace SocialNetworkAppAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly AppDbContext _context;

        public PostController(AppDbContext context, ILogger<PostController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name ="Posts")]
        public async IAsyncEnumerable<Post> GetPosts()
        {
            _logger.LogInformation("GetPosts method started");
            var posts = _context.Posts.AsAsyncEnumerable();
            await foreach (var post in posts)
            {
                yield return post;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostCreateDTO model)
        {
            _logger.LogInformation("CreatePost method started");
            if (ModelState.IsValid)
            {
                var post = new Post();
                post.Title = model.Title;
                if(model.Content != null)
                    post.Content = model.Content;
                post.CreatedDate = DateTime.Now;
                post.LastModifiedDate = DateTime.Now;
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();

                return Ok(post);
            }
            else {
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePost(PostUpdateDTO model) {
            _logger.LogInformation("Update method started");
            if (ModelState.IsValid) { 
                var post = _context.Posts.Where(p => p.Id == model.Id)
                    .FirstOrDefault();
                if (post != null)
                {
                    if (!model.Title.IsNullOrEmpty() && !post.Title.Equals(model.Title))
                        post.Title = model.Title!;
                    if (!model.Content.IsNullOrEmpty())
                        post.Content = model.Content;
                    post.LastModifiedDate = DateTime.Now;
                    _context.Entry(post).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    return Ok(post);
                }
                else {
                    return NotFound();
                }
            } else { return BadRequest(ModelState); }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete method started");
            var post = _context.Posts.Where(p => p.Id == id).FirstOrDefault();
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return Ok(post);
            }
            else {
                return NotFound(ModelState);
            }
        }

    }
}
