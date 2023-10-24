using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAppAPI.DTO
{
    public class PostUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
