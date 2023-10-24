using SocialNetworkAppAPI.DTO;
using SocialNetworkAppAPI.Models;
using SocialNetworkAppAPI.Repositories;
using SocialNetworkAppLibrary.DTO;

namespace SocialNetworkAppAPI.Services;

public interface IPostsService {
    IAsyncEnumerable<Post> GetPosts();
    Task<(Post?, Exception?)> CreatePost(PostCreateDTO model);
    Task<(Post?, Exception?)> UpdatePost(PostUpdateDTO model);
    Task<(Post?, Exception?)> DeletePost(int postId);
}

public class PostsService : IPostsService
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<PostsService> _logger;

    public PostsService(
        IPostRepository postRepository, 
        ILogger<PostsService> logger)
    {
        _postRepository = postRepository;
        _logger = logger;
    }
    public async IAsyncEnumerable<Post> GetPosts()
    {
        var posts = _postRepository.GetPosts();

        await foreach (var post in posts) {
            yield return post;
        }
    }

    public async Task<(Post?, Exception?)> CreatePost(PostCreateDTO model)
    {
        try
        {
            var post = await _postRepository.CreatePostAsync(model);
            return (post, null);
        }
        catch (Exception ex) {
            return (null, ex);
        }
    }

    public async Task<(Post?, Exception?)> UpdatePost(PostUpdateDTO model)
    {
        try
        {
            var post = await _postRepository.UpdatePostAsync(model);
            return (post, null);
        }
        catch (Exception ex)
        {
            return (null, ex);
        }
    }

    public async Task<(Post?, Exception?)> DeletePost(int postId)
    {
        try
        {
            var post = await _postRepository.DeletePostAsync(postId);
            return (post, null);
        }
        catch (Exception ex)
        {
            return (null, ex);
        }
    }


}
