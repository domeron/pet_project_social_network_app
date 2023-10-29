using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using SocialNetworkAppBlazorClient.Models;

namespace SocialNetworkAppBlazorClient.Services;

public interface IHttpService
{
    Task<T> Get<T>(string uri);
    Task<T> Post<T>(string uri, object value);
}
public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    private readonly ILocalStorageService _localStorageService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<HttpService> _logger;

    public HttpService(
        HttpClient httpClient,
        NavigationManager navigationManager,
        ILocalStorageService localStorageService,
        IConfiguration configuration,
        ILogger<HttpService> logger)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _localStorageService = localStorageService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<T> Get<T>(string uri)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest<T>(request);
        }
        catch { throw; }
    }

    public async Task<T> Post<T>(string uri, object value)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return await sendRequest<T>(request);
        }
        catch { throw; };
    }

    private async Task<T> sendRequest<T>(HttpRequestMessage request)
    {
        var user = await _localStorageService.GetItem<User>("user");
        var isApiUrl = request.RequestUri.IsAbsoluteUri;
        Console.WriteLine("not in if");
        if (user != null)
        {
            Console.WriteLine("in if");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
        }
        using var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            throw new Exception(error["message"]);
        }

        return await response.Content.ReadFromJsonAsync<T>();
    }
}
