using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAppAPI.Models
{
    public class Post
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;
        
        public string? Content { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

    }
}
