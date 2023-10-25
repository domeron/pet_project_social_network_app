using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAppLibrary.Data.DTO;
public class PostUpdateDTO
{
    [Required]
    public int Id { get; set; }
    public string? Content { get; set; }
}
