using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SocialNetworkAppBlazorClient;
using SocialNetworkAppBlazorClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = new Uri(builder.Configuration["apiUrl"]!);

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = apiUrl
});

builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IPostsService, PostsService>();

var host = builder.Build();

var authenticationService = host.Services.GetRequiredService <IAuthenticationService>();
await authenticationService.Initialize();

await host.RunAsync();
