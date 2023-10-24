using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAppLibrary.Data.DTO;
public class SignUpDTO
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
