using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ProyectoFinal.DataAccess
{
    public class DBConnection
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["SpotifyDB"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error de conexión: " + ex.Message);
                return false;
            }
        }
    }
}