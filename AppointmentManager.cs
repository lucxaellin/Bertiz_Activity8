using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace GuiForDentalA
{
    public static class AppointmentManager
    {
        // Get appointments for the currently logged-in user
        public static List<Appointment> GetAppointmentsForCurrentUser()
        {
            var appointments = new List<Appointment>();

            string query = @"SELECT appointment_id, patient_id, dentist_id, appointment_date, status, notes 
                             FROM appointments 
                             WHERE patient_id = @PatientId";

            try
            {
                using (var reader = DatabaseHelper.ExecuteQuery(query,
                    new MySqlParameter("@PatientId", UserSession.CurrentUserId)))
                {
                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            AppointmentId = Convert.ToInt32(reader["appointment_id"]),
                            PatientId = Convert.ToInt32(reader["patient_id"]),
                            DentistId = Convert.ToInt32(reader["dentist_id"]),
                            AppointmentDate = Convert.ToDateTime(reader["appointment_date"]),
                            Status = reader["status"].ToString() ?? "",
                            Notes = reader["notes"].ToString() ?? ""
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching appointments: {ex.Message}");
            }

            return appointments;
        }

        // Add new appointment
        public static void AddAppointment(int patientId, int dentistId, DateTime appointmentDate, string status, string notes)
        {
            string query = @"INSERT INTO appointments (patient_id, dentist_id, appointment_date, status, notes)
                             VALUES (@PatientId, @DentistId, @AppointmentDate, @Status, @Notes)";

            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@PatientId", patientId),
                new MySqlParameter("@DentistId", dentistId),
                new MySqlParameter("@AppointmentDate", appointmentDate),
                new MySqlParameter("@Status", status),
                new MySqlParameter("@Notes", notes));
        }

        // Update existing appointment
        public static void UpdateAppointment(int appointmentId, int patientId, int dentistId, DateTime appointmentDate, string status, string notes)
        {
            string query = @"UPDATE appointments
                             SET patient_id = @PatientId,
                                 dentist_id = @DentistId,
                                 appointment_date = @AppointmentDate,
                                 status = @Status,
                                 notes = @Notes
                             WHERE appointment_id = @AppointmentId";

            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@AppointmentId", appointmentId),
                new MySqlParameter("@PatientId", patientId),
                new MySqlParameter("@DentistId", dentistId),
                new MySqlParameter("@AppointmentDate", appointmentDate),
                new MySqlParameter("@Status", status),
                new MySqlParameter("@Notes", notes));
        }

        // Delete appointment
        public static void DeleteAppointment(int appointmentId)
        {
            string query = "DELETE FROM appointments WHERE appointment_id = @AppointmentId";

            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@AppointmentId", appointmentId));
        }
    }
}
