


using SocialNetworkAppLibrary.Data.DTO;
using SocialNetworkAppLibrary.Data.ViewModels;

namespace SocialNetworkAppBlazorClient.Services;

public interface IPostsService
{
    IAsyncEnumerable<PostViewModel> GetAllPosts();
    IAsyncEnumerable<PostViewModel> GetUserPosts(string userId);
    Task CreatePost(PostCreateDTO model);
}

public class PostsService : IPostsService
{
    private readonly IHttpService _httpService;

    public PostsService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async IAsyncEnumerable<PostViewModel> GetAllPosts()
    {
        var posts = await _httpService.Get<IAsyncEnumerable<PostViewModel>>("/posts/all");
        await foreach (var post in posts)
        {
            yield return post;
        }
    }

    public async IAsyncEnumerable<PostViewModel> GetUserPosts(string userId) {
        var posts = await _httpService.Get<IAsyncEnumerable<PostViewModel>>($"/posts/user/{userId}");
        await foreach (var post in posts)
        {
            yield return post;
        }
    }

    public async Task CreatePost(PostCreateDTO model)
    {
        try
        {
            await _httpService.Post<int>("posts/create", model);
        }
        catch { throw; }
    }
}
