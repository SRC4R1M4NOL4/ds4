using System;
using System.Collections.Generic;

namespace ProyectoFinal.Models
{
    // Modelo para canciones de Spotify
    public class Track
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string PreviewUrl { get; set; }
        public string ImageUrl { get; set; }
        public int DurationMs { get; set; }
    }

    // Modelo para historial de búsquedas
    public class SearchHistory
    {
        public int Id { get; set; }
        public string SearchQuery { get; set; }
        public DateTime SearchDate { get; set; }
        public int ResultCount { get; set; }
    }

    // Modelo para preferencias de usuario
    public class UserPreference
    {
        public int Id { get; set; }
        public string TrackId { get; set; }
        public string TrackName { get; set; }
        public string ArtistName { get; set; }
        public DateTime PlayedDate { get; set; }
        public int PlayCount { get; set; }
    }

    // Modelo para reportes
    public class UserReport
    {
        public List<string> TopArtists { get; set; }
        public List<TrackStats> MostPlayedTracks { get; set; }
        public Dictionary<string, int> SearchFrequency { get; set; }
        public int TotalSearches { get; set; }
        public int TotalPlays { get; set; }
        public DateTime? LastActivity { get; set; }
    }

    // Estadísticas de canciones
    public class TrackStats
    {
        public string TrackName { get; set; }
        public string ArtistName { get; set; }
        public int PlayCount { get; set; }
        public DateTime LastPlayed { get; set; }
    }
} 