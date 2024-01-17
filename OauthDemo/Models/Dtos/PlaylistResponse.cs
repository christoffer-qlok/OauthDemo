using System.Text.Json.Serialization;

namespace OauthDemo.Models.Dtos
{
    public class Item
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class PlaylistResponse
    {
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }
    }
}
