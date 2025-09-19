using System.Net.Http;
using System.Threading.Tasks;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Globalization;

namespace ProtectedMcpServer.Tools.Spotify
{
    /// <summary>
    /// MCPServerTools: Provides utility methods for interacting with Spotify playlists.
    /// </summary>
    public class SpotifyPlaylistTools
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string baseUrl = "https://api.spotify.com/v1/playlists/";

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyPlaylistTools"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Factory to create HttpClient instances for network requests.</param>
        public SpotifyPlaylistTools(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Example method to send a network request to Spotify API.
        /// </summary>
        /// <param name="endpoint">Spotify API endpoint.</param>
        /// <returns>Response content as string.</returns>
        /// 
        [McpServerTool, Description("Send a request to the Spotify API.")]
        public async Task<string> GetPlaylistAsync(string id)
        {
            var client = _httpClientFactory.CreateClient();
            
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "<YOUR_SPOTIFY_ACCESS_TOKEN>");
            var endpoint = $"{baseUrl}{id}";

            var response = await client.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        
    }
}