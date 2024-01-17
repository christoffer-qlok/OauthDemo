using System.Text.Json.Serialization;

namespace OauthDemo.Models.Dtos
{
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
