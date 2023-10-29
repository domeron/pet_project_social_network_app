using SocialNetworkAppLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAppLibrary.Data.ViewModels;
public class PostViewModel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }

    public string? Content { get; set; }

    public DateTime CreatedDate { get; set; }

    public PostViewModel(Post post) { 
        this.Id = post.Id;
        this.UserName = post.User.UserName;
        this.UserId = post.UserId;
        this.Content = post.Content;
        this.CreatedDate = post.CreatedDate;
    }

    public PostViewModel() { }

}
