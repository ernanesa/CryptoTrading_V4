using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Trading.Entities;

namespace Trading.Services;

public class AuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;

    public AuthenticationService(HttpClient httpClient, IMemoryCache memoryCache)
    {
        _httpClient = httpClient;
        _memoryCache = memoryCache;
    }

    public async Task<AuthenticationResponse> AuthenticateAsync(User user)
    {
        if (_memoryCache.TryGetValue(user.TypeID, out AuthenticationResponse cachedResponse))
        {
            user.AccessToken = cachedResponse.AccessToken;
            return cachedResponse;
        }

        var request = new AuthenticationRequest
        {
            Login = user.TypeID,
            Password = user.Secret
        };

        var requestContent = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("authorize", requestContent);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var authenticationResponse = JsonSerializer.Deserialize<AuthenticationResponse>(responseContent);

            user.AccessToken = authenticationResponse.AccessToken;

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(authenticationResponse.Expiration)
            };

            _memoryCache.Set(user.TypeID, authenticationResponse, memoryCacheEntryOptions);

            return authenticationResponse;
        }
        else
        {
            throw new HttpRequestException($"Authentication failed: {response.ReasonPhrase}");
        }
    }
}
