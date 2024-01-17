using OauthDemo.Models.Dtos;
using OauthDemo.Models.ViewModel;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;

namespace OauthDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var spotifyClientId = builder.Configuration["SpotifyClientId"];
            var spotifyClientSecret = builder.Configuration["SpotifyClientSecret"];
            var baseUrl = builder.Configuration["BaseUrl"].TrimEnd('/');
            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.MapGet("/auth", () =>
            {
                var parameters = new Dictionary<string, string>()
                {
                    { "client_id", spotifyClientId },
                    { "response_type", "code" },
                    { "redirect_uri", $"{baseUrl}/callback" },
                    { "scope", "playlist-read-private playlist-read-collaborative user-read-private user-read-email" },
                    { "show_dialog", "true" }
                };

                string query = string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                string uri = $"https://accounts.spotify.com/authorize?{query}";
                return Results.Redirect(uri);
            });

            app.MapGet("/callback", async (string? error, string? code) =>
            {
                if (error != null)
                {
                    return Results.Json(error);
                }

                if(code == null)
                {
                    return Results.Json("No code returned");
                }

                // Get access token
                var data = new Dictionary<string, string>()
                {
                    { "grant_type", "authorization_code" },
                    { "code", code },
                    { "redirect_uri", $"{baseUrl}/callback" },
                };

                using (var httpClient = new HttpClient())
                {
                    var body = new FormUrlEncodedContent(data);

                    var request = new HttpRequestMessage() { 
                        Method = HttpMethod.Post, 
                        RequestUri = new Uri("https://accounts.spotify.com/api/token"),
                        Content = body
                    };
                    var clientCredentials = EncodeClientCredentials(spotifyClientId, spotifyClientSecret);
                    request.Headers.Add("Authorization", $"Basic {clientCredentials}");

                    var response = await httpClient.SendAsync(request);
                    string responseData = await response.Content.ReadAsStringAsync();

                    if(!response.IsSuccessStatusCode)
                    {
                        return Results.Json(responseData);
                    }

                    TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseData)!;
                    try
                    {
                        // Get user profile from spotify
                        request = new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = new Uri("https://api.spotify.com/v1/me") };
                        request.Headers.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");

                        response = await httpClient.SendAsync(request);
                        responseData = await response.Content.ReadAsStringAsync()!;
                        ProfileResponse profile = JsonSerializer.Deserialize<ProfileResponse>(responseData)!;

                        // Get playlists for user
                        request = new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = new Uri($"https://api.spotify.com/v1/users/{profile.Id}/playlists") };
                        request.Headers.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");

                        response = await httpClient.SendAsync(request);
                        responseData = await response.Content.ReadAsStringAsync()!;
                        PlaylistResponse playlists = JsonSerializer.Deserialize<PlaylistResponse>(responseData)!;

                        var viewModel = new CallbackViewModel()
                        {
                            DisplayName = profile.DisplayName,
                            Playlists = playlists.Items.Select(i => i.Name).ToArray(),
                        };

                        return Results.Json(viewModel);
                    } catch  (Exception ex)
                    {
                        return Results.Json(ex);
                    }
                }
            });

            app.Run();
        }

        static string EncodeClientCredentials(string clientId, string clientSecret)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
            string base64EncodedString = Convert.ToBase64String(bytesToEncode);
            return base64EncodedString;
        }
    }
}
