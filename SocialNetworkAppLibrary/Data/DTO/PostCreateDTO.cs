using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAppLibrary.Data.DTO;
public class PostCreateDTO
{
    [Required]
    public string UserId { get; set; }
    [Required]

    public string? Content { get; set; }
}
