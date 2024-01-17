using System.Text.Json.Serialization;

namespace OauthDemo.Models.Dtos
{
    public class ProfileResponse
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
