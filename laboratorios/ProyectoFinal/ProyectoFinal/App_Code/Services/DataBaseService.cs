using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ProyectoFinal.DataAccess;
using ProyectoFinal.Models;

namespace ProyectoFinal.Services
{
    public class DatabaseService
    {
        private static DatabaseService _instance;
        private static readonly object _lock = new object();

        private DatabaseService() { }

        public static DatabaseService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseService();
                        }
                    }
                }
                return _instance;
            }
        }

        // Guardar búsqueda
        public void SaveSearch(string query, int resultCount)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string sql = "INSERT INTO SearchHistory (SearchQuery, SearchDate, ResultCount) VALUES (@Query, @Date, @Count)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Query", query);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Count", resultCount);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar búsqueda: " + ex.Message);
            }
        }

        // Registrar reproducción de canción
        public void SavePlayedTrack(string trackId, string trackName, string artistName)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();

                    // Verificar si la canción ya existe
                    string checkSql = "SELECT PlayCount FROM UserPreferences WHERE TrackId = @TrackId";
                    SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@TrackId", trackId);

                    object result = checkCmd.ExecuteScalar();

                    if (result != null)
                    {
                        // Actualizar contador
                        int currentCount = Convert.ToInt32(result);
                        string updateSql = "UPDATE UserPreferences SET PlayCount = @Count, PlayedDate = @Date WHERE TrackId = @TrackId";
                        SqlCommand updateCmd = new SqlCommand(updateSql, conn);
                        updateCmd.Parameters.AddWithValue("@Count", currentCount + 1);
                        updateCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                        updateCmd.Parameters.AddWithValue("@TrackId", trackId);
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // Insertar nuevo registro
                        string insertSql = "INSERT INTO UserPreferences (TrackId, TrackName, ArtistName, PlayedDate, PlayCount) " +
                                         "VALUES (@TrackId, @TrackName, @ArtistName, @Date, 1)";
                        SqlCommand insertCmd = new SqlCommand(insertSql, conn);
                        insertCmd.Parameters.AddWithValue("@TrackId", trackId);
                        insertCmd.Parameters.AddWithValue("@TrackName", trackName);
                        insertCmd.Parameters.AddWithValue("@ArtistName", artistName);
                        insertCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar reproducción: " + ex.Message);
            }
        }

        // Obtener canciones más reproducidas
        public List<TrackStats> GetTopTracks(int limit)
        {
            List<TrackStats> tracks = new List<TrackStats>();

            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string sql = "SELECT TOP (@Limit) TrackName, ArtistName, PlayCount, PlayedDate " +
                               "FROM UserPreferences ORDER BY PlayCount DESC, PlayedDate DESC";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Limit", limit);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        tracks.Add(new TrackStats
                        {
                            TrackName = reader["TrackName"].ToString(),
                            ArtistName = reader["ArtistName"].ToString(),
                            PlayCount = Convert.ToInt32(reader["PlayCount"]),
                            LastPlayed = Convert.ToDateTime(reader["PlayedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener canciones populares: " + ex.Message);
            }

            return tracks;
        }

        // Generar reporte completo
        public UserReport GenerateUserReport()
        {
            UserReport report = new UserReport
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

                    // Top Artistas
                    string artistSql = "SELECT TOP 5 ArtistName, SUM(PlayCount) as TotalPlays " +
                                     "FROM UserPreferences GROUP BY ArtistName ORDER BY TotalPlays DESC";
                    SqlCommand artistCmd = new SqlCommand(artistSql, conn);
                    SqlDataReader artistReader = artistCmd.ExecuteReader();
                    while (artistReader.Read())
                    {
                        report.TopArtists.Add(artistReader["ArtistName"].ToString());
                    }
                    artistReader.Close();

                    // Canciones más reproducidas
                    report.MostPlayedTracks = GetTopTracks(10);

                    // Frecuencia de búsquedas
                    string searchSql = "SELECT TOP 10 SearchQuery, COUNT(*) as Frequency " +
                                     "FROM SearchHistory GROUP BY SearchQuery ORDER BY Frequency DESC";
                    SqlCommand searchCmd = new SqlCommand(searchSql, conn);
                    SqlDataReader searchReader = searchCmd.ExecuteReader();
                    while (searchReader.Read())
                    {
                        report.SearchFrequency.Add(
                            searchReader["SearchQuery"].ToString(),
                            Convert.ToInt32(searchReader["Frequency"])
                        );
                    }
                    searchReader.Close();

                    // Totales
                    string totalSearchesSql = "SELECT COUNT(*) FROM SearchHistory";
                    SqlCommand totalSearchesCmd = new SqlCommand(totalSearchesSql, conn);
                    report.TotalSearches = Convert.ToInt32(totalSearchesCmd.ExecuteScalar());

                    string totalPlaysSql = "SELECT SUM(PlayCount) FROM UserPreferences";
                    SqlCommand totalPlaysCmd = new SqlCommand(totalPlaysSql, conn);
                    object totalPlaysResult = totalPlaysCmd.ExecuteScalar();
                    report.TotalPlays = totalPlaysResult != DBNull.Value ? Convert.ToInt32(totalPlaysResult) : 0;

                    // Última actividad
                    string lastActivitySql = "SELECT MAX(PlayedDate) FROM UserPreferences";
                    SqlCommand lastActivityCmd = new SqlCommand(lastActivitySql, conn);
                    object lastActivityResult = lastActivityCmd.ExecuteScalar();
                    if (lastActivityResult != DBNull.Value)
                    {
                        report.LastActivity = Convert.ToDateTime(lastActivityResult);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar reporte: " + ex.Message);
            }

            return report;
        }
    }
}