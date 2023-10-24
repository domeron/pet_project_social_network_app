using SocialNetworkAppBlazorClient.Models;
using SocialNetworkAppLibrary.DTO;


namespace SocialNetworkAppBlazorClient.Services;

public interface IPostsService {
    Task<List<Post>> GetAllPosts();
    Task CreatePost(PostCreateDTO model);
}

public class PostsService : IPostsService
{
    private readonly IHttpService _httpService;

    public PostsService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<List<Post>> GetAllPosts()
    {
        var posts = await _httpService.Get<List<Post>>("/posts/all");
        return posts;
    }

    public async Task CreatePost(PostCreateDTO model)
    {
        try
        {
            await _httpService.Post<Post>("posts/create", model);
        }
        catch { throw; }
    }
}
