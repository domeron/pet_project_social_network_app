


using SocialNetworkAppLibrary.Data.DTO;
using SocialNetworkAppLibrary.Data.ViewModels;

namespace SocialNetworkAppBlazorClient.Services;

public interface IPostsService
{
    Task<List<PostViewModel>> GetAllPosts();
    Task CreatePost(PostCreateDTO model);
}

public class PostsService : IPostsService
{
    private readonly IHttpService _httpService;

    public PostsService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<List<PostViewModel>> GetAllPosts()
    {
        var posts = await _httpService.Get<List<PostViewModel>>("/posts/all");
        return posts;
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
