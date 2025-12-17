using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using ProyectoFinal.Models;

namespace ProyectoFinal.Services
{
    public class SpotifyService
    {
        private SpotifyClient _spotify;
        private static SpotifyService _instance;
        private static readonly object _lock = new object();

        private SpotifyService()
        {
            InitializeSpotifyClient();
        }

        public static SpotifyService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SpotifyService();
                        }
                    }
                }
                return _instance;
            }
        }

        private void InitializeSpotifyClient()
        {
            try
            {
                    var clientId = ConfigurationManager.AppSettings["e1ce6b1455434cd6bd83e982bb2e04c9"];
                var clientSecret = ConfigurationManager.AppSettings["556af6071fc1453fac97f063923fed7b"];

                if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                {
                    throw new Exception("Faltan las credenciales de Spotify en appSettings (SpotifyClientId/SpotifyClientSecret).");
                }

                var config = SpotifyClientConfig.CreateDefault();
                var oauth = new OAuthClient(config);
                var request = new ClientCredentialsRequest(clientId, clientSecret);
                var response = oauth.RequestToken(request).Result;

                _spotify = new SpotifyClient(config.WithToken(response.AccessToken));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al inicializar Spotify: " + ex.Message, ex);
            }
        }

        // Buscar canciones
        public async Task<List<Track>> SearchTracks(string query)
        {
            try
            {
                var searchRequest = new SearchRequest(SearchRequest.Types.Track, query)
                {
                    Limit = 20
                };

                var searchResponse = await _spotify.Search.Item(searchRequest);

                var tracks = searchResponse.Tracks.Items.Select(track => new Track
                {
                    Id = track.Id,
                    Name = track.Name,
                    Artist = string.Join(", ", track.Artists.Select(a => a.Name)),
                    Album = track.Album?.Name,
                    PreviewUrl = track.PreviewUrl,
                    ImageUrl = track.Album?.Images?.FirstOrDefault()?.Url,
                    DurationMs = track.DurationMs
                }).ToList();

                return tracks;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar canciones: " + ex.Message, ex);
            }
        }

        // Obtener recomendaciones
        public async Task<List<Track>> GetRecommendations(string trackId)
        {
            try
            {
                var recommendationsRequest = new RecommendationsRequest
                {
                    SeedTracks = new List<string> { trackId },
                    Limit = 15
                };

                var recommendations = await _spotify.Browse.GetRecommendations(recommendationsRequest);

                return recommendations.Tracks.Select(track => new Track
                {
                    Id = track.Id,
                    Name = track.Name,
                    Artist = string.Join(", ", track.Artists.Select(a => a.Name)),
                    Album = track.Album?.Name,
                    PreviewUrl = track.PreviewUrl,
                    ImageUrl = track.Album?.Images?.FirstOrDefault()?.Url,
                    DurationMs = track.DurationMs
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener recomendaciones: " + ex.Message, ex);
            }
        }

        // Obtener detalles de una canción
        public async Task<Track> GetTrackDetails(string trackId)
        {
            try
            {
                var track = await _spotify.Tracks.Get(trackId);

                return new Track
                {
                    Id = track.Id,
                    Name = track.Name,
                    Artist = string.Join(", ", track.Artists.Select(a => a.Name)),
                    Album = track.Album?.Name,
                    PreviewUrl = track.PreviewUrl,
                    ImageUrl = track.Album?.Images?.FirstOrDefault()?.Url,
                    DurationMs = track.DurationMs
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener detalles de la canción: " + ex.Message, ex);
            }
        }
    }
}