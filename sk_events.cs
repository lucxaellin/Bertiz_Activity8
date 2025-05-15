using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SkProjectEdP
{
    public partial class sk_events : Form
    {
        public sk_events()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Event ID
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Event Name
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Location
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Event Date
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Organizer ID
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect input values
                string eventName = textBox2.Text.Trim();      // Event Name
                string location = textBox3.Text.Trim();      // Location
                string eventDate = dateTimePicker1.Value.ToString("yyyy-MM-dd"); // Event Date
                string organizerId = textBox4.Text.Trim();   // Organizer ID

                // Validate inputs
                if (string.IsNullOrWhiteSpace(eventName) || string.IsNullOrWhiteSpace(location) || string.IsNullOrWhiteSpace(organizerId))
                {
                    MessageBox.Show("Event Name, Location, and Organizer ID are required fields. Please provide valid inputs.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // SQL query to insert the new event
                string query = "INSERT INTO sk_events (event_name, event_date, location, organizer_id) VALUES (@EventName, @EventDate, @Location, @OrganizerId)";

                // Execute the query using DatabaseHelper
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query,
                    new MySqlParameter("@EventName", eventName),
                    new MySqlParameter("@EventDate", eventDate),
                    new MySqlParameter("@Location", location),
                    new MySqlParameter("@OrganizerId", organizerId));

                // Check if the insertion was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Event added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear input fields
                    textBox2.Clear(); // Event Name
                    textBox3.Clear(); // Location
                    textBox4.Clear(); // Organizer ID
                    dateTimePicker1.Value = DateTime.Now; // Reset to today's date
                }
                else
                {
                    MessageBox.Show("Failed to add event. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the database connection is closed
                DatabaseHelper.CloseConnection();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // SQL query to fetch all events
                string query = "SELECT * FROM sk_events";

                // Execute the query and fetch data
                using (var reader = DatabaseHelper.ExecuteQuery(query))
                {
                    // Create a DataTable and load data
                    DataTable table = new DataTable();
                    table.Load(reader);

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the database connection is closed
                DatabaseHelper.CloseConnection();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect input values
                string eventId = textBox1.Text.Trim();       // Event ID
                string eventName = textBox2.Text.Trim();    // Event Name
                string location = textBox3.Text.Trim();     // Location
                string eventDate = dateTimePicker1.Value.ToString("yyyy-MM-dd"); // Event Date
                string organizerId = textBox4.Text.Trim();  // Organizer ID

                // Validate Event ID
                if (string.IsNullOrWhiteSpace(eventId))
                {
                    MessageBox.Show("Event ID is required to update an event.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate that at least one field to update is provided
                if (string.IsNullOrWhiteSpace(eventName) && string.IsNullOrWhiteSpace(location) &&
                    string.IsNullOrWhiteSpace(eventDate) && string.IsNullOrWhiteSpace(organizerId))
                {
                    MessageBox.Show("At least one field (Event Name, Location, Date, or Organizer ID) must be provided to update the event.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Build the SQL query dynamically
                string query = "UPDATE sk_events SET ";
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                if (!string.IsNullOrWhiteSpace(eventName))
                {
                    query += "event_name = @EventName, ";
                    parameters.Add(new MySqlParameter("@EventName", eventName));
                }
                if (!string.IsNullOrWhiteSpace(location))
                {
                    query += "location = @Location, ";
                    parameters.Add(new MySqlParameter("@Location", location));
                }
                if (!string.IsNullOrWhiteSpace(eventDate))
                {
                    query += "event_date = @EventDate, ";
                    parameters.Add(new MySqlParameter("@EventDate", eventDate));
                }
                if (!string.IsNullOrWhiteSpace(organizerId))
                {
                    query += "organizer_id = @OrganizerId, ";
                    parameters.Add(new MySqlParameter("@OrganizerId", organizerId));
                }

                // Remove trailing comma and space
                query = query.TrimEnd(',', ' ');

                // Add WHERE clause
                query += " WHERE event_id = @EventId";
                parameters.Add(new MySqlParameter("@EventId", eventId));

                // Execute the query
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());

                // Check if the update was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Event updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear input fields
                    textBox1.Clear(); // Event ID
                    textBox2.Clear(); // Event Name
                    textBox3.Clear(); // Location
                    textBox4.Clear(); // Organizer ID
                }
                else
                {
                    MessageBox.Show("No records were updated. Please check the Event ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the database connection is closed
                DatabaseHelper.CloseConnection();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Navigate back to the main form
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect the Event ID from the input field
                string eventId = textBox1.Text.Trim(); // Event ID

                // Validate Event ID
                if (string.IsNullOrWhiteSpace(eventId))
                {
                    MessageBox.Show("Event ID is required to delete an event.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show($"Are you sure you want to delete the event with ID: {eventId}?",
                                                      "Confirm Deletion",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);



                // SQL query to delete the event
                string query = "DELETE FROM sk_events WHERE event_id = @EventId";
                var parameters = new MySqlParameter[]
                {
            new MySqlParameter("@EventId", eventId)
                };

                // Execute the query
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);

                // Check if the deletion was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Event deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear the Event ID input field
                    textBox1.Clear();
                }
                else
                {
                    MessageBox.Show("No records were deleted. Please check if the Event ID exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the database connection is closed
                DatabaseHelper.CloseConnection();
            }
        }
    }
}