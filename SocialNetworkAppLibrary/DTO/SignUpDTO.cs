using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkAppLibrary.DTO;
public class SignUpDTO
{
    [Required]
    [MaxLength(100)]
    [MinLength(2)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(100)]
    [MinLength(2)]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }

}
