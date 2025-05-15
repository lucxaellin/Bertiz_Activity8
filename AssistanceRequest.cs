using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SkProjectEdP
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Optional: Add code to handle label click, or leave empty if not needed
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Retrieve values from text boxes
            if (!int.TryParse(textBox2.Text.Trim(), out int residentId))
            {
                MessageBox.Show("Please enter a valid Resident ID.");
                return;
            }

            if (!int.TryParse(textBox3.Text.Trim(), out int skOfficialId))
            {
                MessageBox.Show("Please enter a valid SK Official ID.");
                return;
            }

            string requestType = comboBox2.SelectedItem?.ToString() ?? "";
            string status = textBox1.Text.Trim();
            DateTime requestDate = dateTimePicker1.Value;

            // Validate required fields
            if (string.IsNullOrWhiteSpace(requestType) || string.IsNullOrWhiteSpace(status))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // SQL query to insert the data
            string query = @"INSERT INTO sk_assistance_requests 
        (resident_id, request_type, status, request_date, sk_official_id) 
        VALUES (@resident_id, @request_type, @status, @request_date, @sk_official_id)";

            // Parameters for the query
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@resident_id", residentId),
                new MySqlParameter("@request_type", requestType),
                new MySqlParameter("@status", status),
                new MySqlParameter("@request_date", requestDate),
                new MySqlParameter("@sk_official_id", skOfficialId)
            };

            try
            {
                // Execute the query
                int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (rows > 0)
                {
                    MessageBox.Show("Request sent successfully!");

                    // Optionally clear fields
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to send request.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            // Create an instance of Form1
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void ClearFields()
        {
            // Clear all input fields after successful submission
            textBox2.Clear();
            textBox3.Clear();
            comboBox2.SelectedIndex = -1;
            textBox1.Clear();
            dateTimePicker1.Value = DateTime.Now;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Type of request
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Status Box
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Date and time picker
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Resident ID Box
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // SK ID
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create an instance of Form1
            Form1 form1 = new Form1();

            // Show Form1
            form1.Show();

            // Close the current Form2
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // SQL query to fetch all assistance requests
                string query = "SELECT * FROM sk_assistance_requests";

                // Execute the query and load data into a DataTable
                using (var reader = DatabaseHelper.ExecuteQuery(query))
                {
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
                if (!int.TryParse(textBox2.Text.Trim(), out int residentId))
                {
                    MessageBox.Show("Please enter a valid Resident ID.");
                    return;
                }

                if (!int.TryParse(textBox3.Text.Trim(), out int skOfficialId))
                {
                    MessageBox.Show("Please enter a valid SK Official ID.");
                    return;
                }

                string requestType = comboBox2.SelectedItem?.ToString() ?? "";
                string status = textBox1.Text.Trim();
                DateTime requestDate = dateTimePicker1.Value;
                string requestId = textBox4.Text.Trim(); // Assume a text box for Request ID

                // Validate Request ID
                if (string.IsNullOrWhiteSpace(requestId))
                {
                    MessageBox.Show("Request ID is required to update the assistance request.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Build the SQL query dynamically
                string query = "UPDATE sk_assistance_requests SET ";
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                if (!string.IsNullOrWhiteSpace(requestType))
                {
                    query += "request_type = @RequestType, ";
                    parameters.Add(new MySqlParameter("@RequestType", requestType));
                }
                if (!string.IsNullOrWhiteSpace(status))
                {
                    query += "status = @Status, ";
                    parameters.Add(new MySqlParameter("@Status", status));
                }
                query = query.TrimEnd(',', ' '); // Remove trailing comma
                query += " WHERE request_id = @RequestId";
                parameters.Add(new MySqlParameter("@RequestId", requestId));

                // Execute the query
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Assistance request updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No records were updated. Please check the Request ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect the Request ID from the input field
                string requestId = textBox4.Text.Trim(); // Assume a text box for Request ID

                // Validate Request ID
                if (string.IsNullOrWhiteSpace(requestId))
                {
                    MessageBox.Show("Request ID is required to delete the assistance request.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show($"Are you sure you want to delete the assistance request with ID: {requestId}?",
                                                      "Confirm Deletion",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Build the SQL DELETE query
                string query = "DELETE FROM sk_assistance_requests WHERE request_id = @RequestId";
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@RequestId", requestId)
                };

                // Execute the query
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Assistance request deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally refresh the DataGridView
                    button4_Click(sender, e); // Calls the "View Assistance" button logic to reload data
                }
                else
                {
                    MessageBox.Show("No records were deleted. Please check if the Request ID exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Populate fields based on the selected row in the DataGridView
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Populate text boxes with data from the selected row
                textBox4.Text = row.Cells["request_id"].Value?.ToString(); // Request ID
                textBox2.Text = row.Cells["resident_id"].Value?.ToString(); // Resident ID
                textBox3.Text = row.Cells["sk_official_id"].Value?.ToString(); // SK Official ID
                comboBox2.SelectedItem = row.Cells["request_type"].Value?.ToString(); // Request Type
                textBox1.Text = row.Cells["status"].Value?.ToString(); // Status
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["request_date"].Value); // Request Date
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}