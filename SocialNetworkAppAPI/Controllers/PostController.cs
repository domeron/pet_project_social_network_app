using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialNetworkAppAPI.DTO;
using SocialNetworkAppAPI.Models;

namespace SocialNetworkAppAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name ="Posts")]
        public async IAsyncEnumerable<Post> GetPosts()
        {
            var posts = _context.Posts.AsAsyncEnumerable();
            await foreach (var post in posts)
            {
                yield return post;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostCreateDTO model)
        {
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
