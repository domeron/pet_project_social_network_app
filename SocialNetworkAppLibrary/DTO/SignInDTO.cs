using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAppLibrary.DTO;
public class SignInDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MaxLength(100), MinLength(2)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
