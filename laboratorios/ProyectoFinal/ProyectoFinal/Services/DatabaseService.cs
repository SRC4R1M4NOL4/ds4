using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProyectoFinal.Services
{
    // Implementación colocada en carpeta Services para que quede incluida en el proyecto Web Application
    public sealed class DatabaseService
    {
        private static readonly Lazy<DatabaseService> _lazy = new Lazy<DatabaseService>(() => new DatabaseService());
        public static DatabaseService Instance => _lazy.Value;

        private DatabaseService() { }

        public void SaveSearch(string query, int resultCount)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    const string sql = "INSERT INTO SearchHistory (SearchQuery, SearchDate, ResultCount) VALUES (@Query, @Date, @Count)";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Query", query ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Count", resultCount);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar búsqueda: " + ex.Message, ex);
            }
        }

        public void SavePlayedTrack(string trackId, string trackName, string artistName)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();

                    const string checkSql = "SELECT PlayCount FROM UserPreferences WHERE TrackId = @TrackId";
                    using (var checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@TrackId", trackId);
                        var result = checkCmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            int currentCount = Convert.ToInt32(result);
                            const string updateSql = "UPDATE UserPreferences SET PlayCount = @Count, PlayedDate = @Date WHERE TrackId = @TrackId";
                            using (var updateCmd = new SqlCommand(updateSql, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@Count", currentCount + 1);
                                updateCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                                updateCmd.Parameters.AddWithValue("@TrackId", trackId);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            const string insertSql = "INSERT INTO UserPreferences (TrackId, TrackName, ArtistName, PlayedDate, PlayCount) VALUES (@TrackId, @TrackName, @ArtistName, @Date, 1)";
                            using (var insertCmd = new SqlCommand(insertSql, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@TrackId", trackId);
                                insertCmd.Parameters.AddWithValue("@TrackName", trackName ?? string.Empty);
                                insertCmd.Parameters.AddWithValue("@ArtistName", artistName ?? string.Empty);
                                insertCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar reproducción: " + ex.Message, ex);
            }
        }

        public List<TrackStats> GetTopTracks(int limit)
        {
            var tracks = new List<TrackStats>();
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    const string sql = "SELECT TOP (@Limit) TrackName, ArtistName, PlayCount, PlayedDate FROM UserPreferences ORDER BY PlayCount DESC, PlayedDate DESC";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Limit", limit);
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tracks.Add(new TrackStats
                                {
                                    TrackName = reader["TrackName"].ToString(),
                                    ArtistName = reader["ArtistName"].ToString(),
                                    PlayCount = Convert.ToInt32(reader["PlayCount"]),
                                    LastPlayed = reader["PlayedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["PlayedDate"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener canciones populares: " + ex.Message, ex);
            }
            return tracks;
        }

        public UserReport GenerateUserReport()
        {
            var report = new UserReport
            {
                TopArtists = new List<string>(),
                MostPlayedTracks = new List<TrackStats>(),
                SearchFrequency = new Dictionary<string, int>()
            };

            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();

                    // Top artistas
                    const string artistSql = "SELECT TOP 5 ArtistName, SUM(PlayCount) as TotalPlays FROM UserPreferences GROUP BY ArtistName ORDER BY TotalPlays DESC";
                    using (var artistCmd = new SqlCommand(artistSql, conn))
                    using (var artistReader = artistCmd.ExecuteReader())
                    {
                        while (artistReader.Read())
                        {
                            report.TopArtists.Add(artistReader["ArtistName"].ToString());
                        }
                    }

                    // Canciones más reproducidas
                    report.MostPlayedTracks = GetTopTracks(10);

                    // Frecuencia de búsquedas
                    const string searchSql = "SELECT TOP 10 SearchQuery, COUNT(*) as Frequency FROM SearchHistory GROUP BY SearchQuery ORDER BY Frequency DESC";
                    using (var searchCmd = new SqlCommand(searchSql, conn))
                    using (var searchReader = searchCmd.ExecuteReader())
                    {
                        while (searchReader.Read())
                        {
                            report.SearchFrequency.Add(searchReader["SearchQuery"].ToString(), Convert.ToInt32(searchReader["Frequency"]));
                        }
                    }

                    // Totales
                    const string totalSearchesSql = "SELECT COUNT(*) FROM SearchHistory";
                    using (var totalSearchesCmd = new SqlCommand(totalSearchesSql, conn))
                    {
                        report.TotalSearches = Convert.ToInt32(totalSearchesCmd.ExecuteScalar());
                    }

                    const string totalPlaysSql = "SELECT SUM(PlayCount) FROM UserPreferences";
                    using (var totalPlaysCmd = new SqlCommand(totalPlaysSql, conn))
                    {
                        var totalPlaysResult = totalPlaysCmd.ExecuteScalar();
                        report.TotalPlays = totalPlaysResult != DBNull.Value ? Convert.ToInt32(totalPlaysResult) : 0;
                    }

                    const string lastActivitySql = "SELECT MAX(PlayedDate) FROM UserPreferences";
                    using (var lastActivityCmd = new SqlCommand(lastActivitySql, conn))
                    {
                        var lastActivityResult = lastActivityCmd.ExecuteScalar();
                        if (lastActivityResult != DBNull.Value)
                        {
                            report.LastActivity = Convert.ToDateTime(lastActivityResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar reporte: " + ex.Message, ex);
            }

            return report;
        }
    }
}