using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAppLibrary.DTO;
public class PostCreateDTO
{
    [Required]
    public string Title { get; set; }

    public string? Content { get; set; }
}
