using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SkProjectEdP
{
    public partial class AddBudgetcs : Form
    {
        public AddBudgetcs()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Budget ID
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //aalocation type 

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //amount

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        { //date

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect input values from the textboxes and date picker
                string allocationType = textBox2.Text.Trim();      // Allocation Type
                string amount = textBox3.Text.Trim();             // Amount
                string allocationDate = dateTimePicker1.Value.ToString("yyyy-MM-dd"); // Allocation Date
                string skId = textBox4.Text.Trim();               // SK Official ID

                // Validate inputs
                if (string.IsNullOrWhiteSpace(allocationType) || string.IsNullOrWhiteSpace(amount) || string.IsNullOrWhiteSpace(skId))
                {
                    MessageBox.Show("Allocation Type, Amount, and SK Official ID are required fields. Please provide valid inputs.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!decimal.TryParse(amount, out decimal parsedAmount) || parsedAmount <= 0)
                {
                    MessageBox.Show("Amount must be a valid positive number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // SQL query to insert the new budget
                string query = "INSERT INTO sk_budget (allocation_type, amount, allocation_date, sk_official_id) VALUES (@AllocationType, @Amount, @AllocationDate, @SkId)";

                // Execute the query using DatabaseHelper
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query,
                    new MySqlParameter("@AllocationType", allocationType),
                    new MySqlParameter("@Amount", parsedAmount),
                    new MySqlParameter("@AllocationDate", allocationDate),
                    new MySqlParameter("@SkId", skId));

                // Check if the insertion was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Budget added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally clear the input fields
                    textBox2.Clear(); // Allocation Type
                    textBox3.Clear(); // Amount
                    textBox4.Clear(); // SK Official ID
                    dateTimePicker1.Value = DateTime.Now; // Reset to today's date
                }
                else
                {
                    MessageBox.Show("Failed to add budget. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //sk id
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect input values from the textboxes and date picker
                string budgetId = textBox1.Text.Trim();       // Budget ID
                string allocationType = textBox2.Text.Trim(); // Allocation Type
                string amount = textBox3.Text.Trim();        // Amount
                string allocationDate = dateTimePicker1.Value.ToString("yyyy-MM-dd"); // Allocation Date
                string skId = textBox4.Text.Trim();          // SK Official ID

                // Validate budget ID
                if (string.IsNullOrWhiteSpace(budgetId))
                {
                    MessageBox.Show("Budget ID is required to update the budget.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate that at least one field to update is provided
                if (string.IsNullOrWhiteSpace(allocationType) && string.IsNullOrWhiteSpace(amount) &&
                    string.IsNullOrWhiteSpace(allocationDate) && string.IsNullOrWhiteSpace(skId))
                {
                    MessageBox.Show("At least one field (Allocation Type, Amount, Date, or SK Official ID) must be provided to update the budget.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Build the SQL query dynamically based on provided inputs
                string query = "UPDATE sk_budget SET ";
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                if (!string.IsNullOrWhiteSpace(allocationType))
                {
                    query += "allocation_type = @AllocationType, ";
                    parameters.Add(new MySqlParameter("@AllocationType", allocationType));
                }
                if (!string.IsNullOrWhiteSpace(amount))
                {
                    if (!decimal.TryParse(amount, out decimal parsedAmount) || parsedAmount <= 0)
                    {
                        MessageBox.Show("Amount must be a valid positive number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    query += "amount = @Amount, ";
                    parameters.Add(new MySqlParameter("@Amount", parsedAmount));
                }
                if (!string.IsNullOrWhiteSpace(allocationDate))
                {
                    query += "allocation_date = @AllocationDate, ";
                    parameters.Add(new MySqlParameter("@AllocationDate", allocationDate));
                }
                if (!string.IsNullOrWhiteSpace(skId))
                {
                    query += "sk_official_id = @SkId, ";
                    parameters.Add(new MySqlParameter("@SkId", skId));
                }

                // Remove the trailing comma and space
                query = query.TrimEnd(',', ' ');

                // Add the WHERE clause to target the specific budget ID
                query += " WHERE budget_id = @BudgetId";
                parameters.Add(new MySqlParameter("@BudgetId", budgetId));

                // Execute the query using DatabaseHelper
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());

                // Check if the update was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Budget updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally, clear the input fields
                    textBox1.Clear(); // Budget ID
                    textBox2.Clear(); // Allocation Type
                    textBox3.Clear(); // Amount
                    textBox4.Clear(); // SK Official ID
                }
                else
                {
                    MessageBox.Show("No records were updated. Please check the Budget ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                string query = "SELECT * FROM sk_budget";
                using (var reader = DatabaseHelper.ExecuteQuery(query))
                {
                    // Create a new DataTable object
                    DataTable table = new DataTable();

                    // Load the data from the reader into the DataTable
                    table.Load(reader);

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                // Display an error message if something goes wrong
                MessageBox.Show($"An error occurred while fetching data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the database connection is closed
                DatabaseHelper.CloseConnection();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();

            // Show Form1
            form1.Show();

            // Close the current Form2
            this.Close();
        }

        private void AddBudgetcs_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            try
            {
                // Collect the Budget ID from the input field
                string budgetId = textBox1.Text.Trim(); // Budget ID

                // Validate Budget ID
                if (string.IsNullOrWhiteSpace(budgetId))
                {
                    MessageBox.Show("Budget ID is required to delete the budget.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show($"Are you sure you want to delete the budget with ID: {budgetId}?",
                                                      "Confirm Deletion",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Build the SQL DELETE query
                string query = "DELETE FROM sk_budget WHERE budget_id = @BudgetId";
                var parameters = new MySqlParameter[]
                {
            new MySqlParameter("@BudgetId", budgetId)
                };

                // Execute the query using DatabaseHelper
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);

                // Check if the deletion was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Budget deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally, clear the input fields
                    textBox1.Clear(); // Budget ID
                    textBox2.Clear(); // Allocation Type
                    textBox3.Clear(); // Amount
                    textBox4.Clear(); // SK Official ID
                }
                else
                {
                    MessageBox.Show("No records were deleted. Please check if the Budget ID exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
