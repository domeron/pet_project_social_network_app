using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAppLibrary.Data.ViewModels;
public class PostViewModel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }

}
