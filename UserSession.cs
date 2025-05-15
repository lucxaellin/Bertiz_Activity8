using System;
using MySql.Data.MySqlClient;

namespace GuiForDentalA
{
    public static class UserSession
    {
        public static int CurrentUserId { get; private set; }
        public static string FirstName { get; private set; } = string.Empty;
        public static string LastName { get; private set; } = string.Empty;
        public static string Email { get; private set; } = string.Empty;
        public static string Phone { get; private set; } = string.Empty;

        // Method to set the current user by patient ID and fetch their details
        public static void SetCurrentUser(int patientId)
        {
            CurrentUserId = patientId;

            try
            {
                // Query to fetch user details
                string query = "SELECT first_name, last_name, email, phone FROM patients WHERE patient_id = @PatientId";

                // Execute the query using DatabaseHelper
                using (var reader = DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@PatientId", patientId)))
                {
                    if (reader.Read())
                    {
                        // Store the user's details in the UserSession properties
                        FirstName = reader["first_name"].ToString() ?? "Unknown";
                        LastName = reader["last_name"].ToString() ?? "Unknown";
                        Email = reader["email"].ToString() ?? "Unknown";
                        Phone = reader["phone"].ToString() ?? "Unknown";

                        Console.WriteLine($"CurrentUserId set to: {CurrentUserId}, Name: {FirstName} {LastName}");
                    }
                    else
                    {
                        throw new Exception($"No user found with ID: {CurrentUserId}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user details: {ex.Message}");
                throw new Exception($"Error fetching user details: {ex.Message}");
            }
        }
    }
}

