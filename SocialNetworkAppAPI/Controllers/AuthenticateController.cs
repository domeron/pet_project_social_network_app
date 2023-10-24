using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialNetworkAppAPI.Authentication;
using SocialNetworkAppAPI.Models;
using SocialNetworkAppLibrary.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialNetworkAppAPI.Controllers;

[Route("auth")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly UserManager<ApiUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticateController> _logger;

    public AuthenticateController(
        UserManager<ApiUser> userManager, 
        RoleManager<IdentityRole> roleManager, 
        IConfiguration configuration,
        ILogger<AuthenticateController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInDTO model)
    { 
        var user = await _userManager.FindByEmailAsync(model.Email);
        _logger.LogInformation("user", user);
        if (user != null)
        {
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return BadRequest( new { 
                    message = "Wrong Password"
                });
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles) {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            
            return Ok(new
            {
                Id = user.Id, UserName = user.UserName, Email = user.Email,
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        return Unauthorized(new {
            message = "We couldn't find users with provided email"
        });
    }

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpDTO model)
    { 
        if(await _userManager.FindByEmailAsync(model.Email) != null
            || await _userManager.FindByNameAsync(model.UserName) != null)
            return BadRequest(new {
                message="User with provided email or username already exists!"});
        ApiUser user = new ApiUser()
        {
            Email = model.Email,
            UserName = model.UserName,
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "User creation failed! Please check user details and try again." });
        return Ok(new { message = "User created successfully!" });

    }
}
