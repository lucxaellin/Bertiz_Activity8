using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SkProjectEdP
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString = "server=localhost;database=sk_bombon_barangay_management_system;uid=root;pwd=rockglamouro5;";
        private static MySqlConnection? connection;

        // Get or create singleton connection
        public static MySqlConnection GetConnection()
        {
            if (connection == null)
            {
                connection = new MySqlConnection(connectionString);
            }

            if (connection.State == System.Data.ConnectionState.Closed || connection.State == System.Data.ConnectionState.Broken)
            {
                connection.Open();
            }

            return connection;
        }

        // Test DB connection
        public static void TestDatabaseConnection()
        {
            try
            {
                using (var cnn = new MySqlConnection(connectionString))
                {
                    cnn.Open();
                    MessageBox.Show("SK Bombon DB Connection Open!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot open SK Bombon DB connection! Error: {ex.Message}");
            }
        }

        // Execute SELECT or similar query
        public static MySqlDataReader ExecuteQuery(string query, params MySqlParameter[] parameters)
        {
            try
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();

                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing query: {ex.Message}");
            }
        }

        // Execute INSERT, UPDATE, DELETE
        public static int ExecuteNonQuery(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (var cmd = new MySqlCommand(query, GetConnection()))
                {
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing non-query: {ex.Message}");
            }
        }

        // Close the connection
        public static void CloseConnection()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}