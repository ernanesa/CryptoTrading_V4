using System.Text.Json.Serialization;

namespace Trading.Entities
{
    public class AuthenticationRequest
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    public class AuthenticationResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("expiration")]
        public long Expiration { get; set; }
    }
}