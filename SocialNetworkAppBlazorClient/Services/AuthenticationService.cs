using Microsoft.AspNetCore.Components;
using SocialNetworkAppBlazorClient.Models;
using SocialNetworkAppLibrary.Data.DTO;

namespace SocialNetworkAppBlazorClient.Services;

public interface IAuthenticationService
{
    User? User { get; }
    event EventHandler<UserSignedInEventArgs> UserSignedIn;
    Task Initialize();
    Task SignIn(string email, string password);
    Task SignUp(SignUpDTO model);
    Task SignOut();
    void UserIsSignedIn(UserSignedInEventArgs args);
}
public class AuthenticationService : IAuthenticationService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IHttpService _httpService;
    private readonly NavigationManager _navigationManager;
    private readonly ILogger<AuthenticationService> _logger;
    public event EventHandler<UserSignedInEventArgs> UserSignedIn;

    public User? User { get; private set; } = null;

    public AuthenticationService(
        ILocalStorageService localStorageService,
        IHttpService httpService,
        NavigationManager navigationManager,
        ILogger<AuthenticationService> logger)
    {
        _localStorageService = localStorageService;
        _httpService = httpService;
        _navigationManager = navigationManager;
        _logger = logger;
    }

    public async Task Initialize()
    {
        User = await _localStorageService.GetItem<User?>("user");
    }

    public async Task SignIn(string email, string password)
    {
        try
        {
            User = await _httpService.Post<User>("/auth/signin",
                new SignInDTO { Email = email, Password = password });
        }
        catch { throw; }
        await _localStorageService.SetItem("user", User);

        UserIsSignedIn(new UserSignedInEventArgs { isSignedIn = true });
    }

    public async Task SignUp(SignUpDTO model)
    {
        try
        {
            await _httpService.Post<Dictionary<string, string>>("/auth/signup", model);
        }
        catch { throw; }
    }

    public async Task SignOut()
    {
        User = null;
        await _localStorageService.RemoveItem("user");
        UserIsSignedIn(new UserSignedInEventArgs { isSignedIn = false });
        _navigationManager.NavigateTo("/signin");
    }

    public void UserIsSignedIn(UserSignedInEventArgs args)
    {
        UserSignedIn?.Invoke(this, args);
    }
}

public class UserSignedInEventArgs : EventArgs
{
    public bool isSignedIn { get; set; }
}
