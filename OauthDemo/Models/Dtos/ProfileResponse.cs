using System.Text.Json.Serialization;

namespace OauthDemo.Models.Dtos
{
    public class ExplicitContent
    {
        [JsonPropertyName("filter_enabled")]
        public bool FilterEnabled { get; set; }

        [JsonPropertyName("filter_locked")]
        public bool FilterLocked { get; set; }
    }

    public class ExternalUrls
    {
        [JsonPropertyName("spotify")]
        public string Spotify { get; set; }
    }

    public class ProfileResponse
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("explicit_content")]
        public ExplicitContent ExplicitContent { get; set; }

        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonPropertyName("followers")]
        public Followers Followers { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("images")]
        public List<Image> Images { get; set; }

        [JsonPropertyName("product")]
        public string Product { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }


}
