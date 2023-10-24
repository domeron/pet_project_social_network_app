using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkAppLibrary.Data.DTO;
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
