using Microsoft.AspNetCore.Identity;
using SocialNetworkAppLibrary.Data.Models;

namespace SocialNetworkAppLibrary.Data.Models;

public class ApiUser : IdentityUser
{
    public ICollection<Post> Posts { get; set; }
}
