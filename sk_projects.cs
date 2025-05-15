using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SkProjectEdP
{
    public partial class sk_projects : Form
    {
        public sk_projects()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Project ID
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Project Name
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Description
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Start Date
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Location
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // Budget
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // Organizer ID
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect input values
                string projectName = textBox2.Text.Trim();      // Project Name
                string description = textBox3.Text.Trim();     // Description
                string project_date = dateTimePicker1.Value.ToString("yyyy-MM-dd"); // Start Date
                string location = textBox4.Text.Trim();        // Location
                string budget = textBox5.Text.Trim();          // Budget
                string organizerId = textBox6.Text.Trim();     // Organizer ID

                // Validate inputs
                if (string.IsNullOrWhiteSpace(projectName) || string.IsNullOrWhiteSpace(description) ||
                    string.IsNullOrWhiteSpace(location) || string.IsNullOrWhiteSpace(budget) || string.IsNullOrWhiteSpace(organizerId))
                {
                    MessageBox.Show("All fields are required. Please provide valid inputs.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!decimal.TryParse(budget, out decimal parsedBudget) || parsedBudget <= 0)
                {
                    MessageBox.Show("Budget must be a valid positive number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // SQL query to insert the new project
                string query = "INSERT INTO sk_projects (project_name, description, project_date, location, budget, organizer_id) " +
                               "VALUES (@ProjectName, @Description, @project_date, @Location, @Budget, @OrganizerId)";

                // Execute the query using DatabaseHelper
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query,
                    new MySqlParameter("@ProjectName", projectName),
                    new MySqlParameter("@Description", description),
                    new MySqlParameter("@project_date", project_date),
                    new MySqlParameter("@Location", location),
                    new MySqlParameter("@Budget", parsedBudget),
                    new MySqlParameter("@OrganizerId", organizerId));

                // Check if the insertion was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Project added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear input fields
                    textBox2.Clear(); // Project Name
                    textBox3.Clear(); // Description
                    textBox4.Clear(); // Location
                    textBox5.Clear(); // Budget
                    textBox6.Clear(); // Organizer ID
                    dateTimePicker1.Value = DateTime.Now; // Reset to today's date
                }
                else
                {
                    MessageBox.Show("Failed to add project. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                // SQL query to fetch all projects
                string query = "SELECT * FROM sk_projects";

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
                string projectId = textBox1.Text.Trim();       // Project ID
                string projectName = textBox2.Text.Trim();    // Project Name
                string description = textBox3.Text.Trim();    // Description
                string project_date = dateTimePicker1.Value.ToString("yyyy-MM-dd"); // Start Date
                string location = textBox4.Text.Trim();       // Location
                string budget = textBox5.Text.Trim();         // Budget
                string organizerId = textBox6.Text.Trim();    // Organizer ID

                // Validate Project ID
                if (string.IsNullOrWhiteSpace(projectId))
                {
                    MessageBox.Show("Project ID is required to update a project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate that at least one field to update is provided
                if (string.IsNullOrWhiteSpace(projectName) && string.IsNullOrWhiteSpace(description) &&
                    string.IsNullOrWhiteSpace(project_date) && string.IsNullOrWhiteSpace(location) &&
                    string.IsNullOrWhiteSpace(budget) && string.IsNullOrWhiteSpace(organizerId))
                {
                    MessageBox.Show("At least one field (Project Name, Description, Date, Location, Budget, or Organizer ID) must be provided to update the project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Build the SQL query dynamically
                string query = "UPDATE sk_projects SET ";
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                if (!string.IsNullOrWhiteSpace(projectName))
                {
                    query += "project_name = @ProjectName, ";
                    parameters.Add(new MySqlParameter("@ProjectName", projectName));
                }
                if (!string.IsNullOrWhiteSpace(description))
                {
                    query += "description = @Description, ";
                    parameters.Add(new MySqlParameter("@Description", description));
                }
                if (!string.IsNullOrWhiteSpace(project_date))
                {
                    query += "project_date= @project_date, ";
                    parameters.Add(new MySqlParameter("@project_date", project_date));
                }
                if (!string.IsNullOrWhiteSpace(location))
                {
                    query += "location = @Location, ";
                    parameters.Add(new MySqlParameter("@Location", location));
                }
                if (!string.IsNullOrWhiteSpace(budget))
                {
                    if (!decimal.TryParse(budget, out decimal parsedBudget) || parsedBudget <= 0)
                    {
                        MessageBox.Show("Budget must be a valid positive number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    query += "budget = @Budget, ";
                    parameters.Add(new MySqlParameter("@Budget", parsedBudget));
                }
                if (!string.IsNullOrWhiteSpace(organizerId))
                {
                    query += "organizer_id = @OrganizerId, ";
                    parameters.Add(new MySqlParameter("@OrganizerId", organizerId));
                }

                // Remove trailing comma and space
                query = query.TrimEnd(',', ' ');

                // Add WHERE clause
                query += " WHERE project_id = @ProjectId";
                parameters.Add(new MySqlParameter("@ProjectId", projectId));

                // Execute the query
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());

                // Check if the update was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Project updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear input fields
                    textBox1.Clear(); // Project ID
                    textBox2.Clear(); // Project Name
                    textBox3.Clear(); // Description
                    textBox4.Clear(); // Location
                    textBox5.Clear(); // Budget
                    textBox6.Clear(); // Organizer ID
                }
                else
                {
                    MessageBox.Show("No records were updated. Please check the Project ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                // Collect the Project ID from the input field
                string projectId = textBox1.Text.Trim(); // Project ID

                // Validate Project ID
                if (string.IsNullOrWhiteSpace(projectId))
                {
                    MessageBox.Show("Project ID is required to delete a project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show($"Are you sure you want to delete the project with ID: {projectId}?",
                                                      "Confirm Deletion",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

               

                // SQL query to delete the project
                string query = "DELETE FROM sk_projects WHERE project_id = @ProjectId";
                var parameters = new MySqlParameter[]
                {
            new MySqlParameter("@ProjectId", projectId)
                };

                // Execute the query
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);

                // Check if the deletion was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Project deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear the Project ID input field
                    textBox1.Clear();
                }
                else
                {
                    MessageBox.Show("No records were deleted. Please check if the Project ID exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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