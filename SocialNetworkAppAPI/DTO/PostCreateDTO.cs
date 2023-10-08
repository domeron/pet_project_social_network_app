using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAppAPI.DTO
{
    public class PostCreateDTO
    {
        [Required]
        public string Title { get; set; }

        public string? Content { get; set; }
    }
}
