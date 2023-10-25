using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkAppLibrary.Data.Models;
public class Post
{

    [Required]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    public string? Content { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }

    public ApiUser User { get; set; }
}
