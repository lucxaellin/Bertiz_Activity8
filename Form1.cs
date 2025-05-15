using System;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace SkProjectEdP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadTable("households");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadTable("residents");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadTable("sk_budget");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadTable("sk_projects");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadTable("sk_events");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoadTable("sk_officials");
            if (dataGridView1.Columns.Contains("password"))
            {
                dataGridView1.Columns["password"].Visible = false;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            LoadTable("sk_bombon_barangay_management_system.sk_assistance_requests");
        }

        private void LoadTable(string tableName)
        {
            try
            {
                string query = $"SELECT * FROM {tableName}";
                using (var reader = DatabaseHelper.ExecuteQuery(query))
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading {tableName}: {ex.Message}");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form2 assistanceRequestForm = new Form2();
            assistanceRequestForm.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form addBudgetForm = new AddBudgetcs();
            addBudgetForm.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form sk_projects = new sk_projects();
            sk_projects.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form sk_eventsForm = new sk_events();
            sk_eventsForm.Show();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns.Contains("password"))
            {
                dataGridView1.Columns["password"].Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Optional: handle form load
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: handle combo box selection
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            // Get the selected row
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            // Initialize variables for ID column and value
            string idColumn = null;
            object idValue = null;
            string tableName = null;

            // Check for each potential ID column and assign corresponding table name
            if (selectedRow.Cells["request_id"]?.Value != null && selectedRow.Cells["request_id"].Value != DBNull.Value)
            {
                idColumn = "request_id";
                idValue = selectedRow.Cells["request_id"].Value;
                tableName = "requests"; // Corresponding table
            }
            else if (selectedRow.Cells["event_id"]?.Value != null && selectedRow.Cells["event_id"].Value != DBNull.Value)
            {
                idColumn = "event_id";
                idValue = selectedRow.Cells["event_id"].Value;
                tableName = "events"; // Corresponding table
            }
            else if (selectedRow.Cells["budget_id"]?.Value != null && selectedRow.Cells["budget_id"].Value != DBNull.Value)
            {
                idColumn = "budget_id";
                idValue = selectedRow.Cells["budget_id"].Value;
                tableName = "budgets"; // Corresponding table
            }
            else if (selectedRow.Cells["project_id"]?.Value != null && selectedRow.Cells["project_id"].Value != DBNull.Value)
            {
                idColumn = "project_id";
                idValue = selectedRow.Cells["project_id"].Value;
                tableName = "projects"; // Corresponding table
            }
            else if (selectedRow.Cells["households_id"]?.Value != null && selectedRow.Cells["households_id"].Value != DBNull.Value)
            {
                idColumn = "households_id";
                idValue = selectedRow.Cells["households_id"].Value;
                tableName = "households"; // Corresponding table
            }
            else if (selectedRow.Cells["residents_id"]?.Value != null && selectedRow.Cells["residents_id"].Value != DBNull.Value)
            {
                idColumn = "residents_id";
                idValue = selectedRow.Cells["residents_id"].Value;
                tableName = "residents"; // Corresponding table
            }

            // Validate if an ID column and table name were found
            if (idColumn == null || idValue == null || tableName == null)
            {
                MessageBox.Show("Unable to find a valid ID in the selected row.");
                return;
            }

            // Confirm deletion
            DialogResult result = MessageBox.Show($"Are you sure you want to delete this row with {idColumn}: {idValue}?",
                                                  "Confirm Deletion",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
            {
                return;
            }

            // Delete query
            string query = $"DELETE FROM {tableName} WHERE {idColumn} = @ID";

            var parameters = new MySql.Data.MySqlClient.MySqlParameter[]
            {
        new MySql.Data.MySqlClient.MySqlParameter("@ID", idValue)
            };

            try
            {
                // Execute the delete query
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    // Remove the row from the DataGridView
                    dataGridView1.Rows.Remove(selectedRow);

                    MessageBox.Show("Row deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to delete the row.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        /// <summary>
        /// Returns the table name associated with the given ID column.
        /// </summary>

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if there is data to export
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("No data available to export.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Assume the table name is stored in a variable (you can adjust this logic as needed)
                string tableName = "exported_table"; // Default table name
                if (dataGridView1.DataSource is DataTable dataTableSource)
                {
                    tableName = dataTableSource.TableName; // Use the table name if available
                }

                // Create a DataTable from the DataGridView
                DataTable dataTable = new DataTable();
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    dataTable.Columns.Add(column.HeaderText); // Add column headers to the DataTable
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        dataRow[cell.ColumnIndex] = cell.Value?.ToString(); // Add cell values to the DataTable
                    }
                    dataTable.Rows.Add(dataRow);
                }

                // Prompt the user to select a save location
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "Save as Excel File";
                    saveFileDialog.FileName = $"{tableName}.xlsx"; // Set file name to the table name

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Create an Excel file using ClosedXML
                        using (var workbook = new ClosedXML.Excel.XLWorkbook())
                        {
                            workbook.Worksheets.Add(dataTable, tableName); // Name the worksheet after the table
                            workbook.SaveAs(saveFileDialog.FileName);
                        }

                        MessageBox.Show("Data exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                // Define fonts and layout for the print
                Font headerFont = new Font("Arial", 12, FontStyle.Bold);
                Font cellFont = new Font("Arial", 10);
                Brush brush = Brushes.Black;

                int startX = 50; // Margin from the left
                int startY = 50; // Margin from the top
                int cellHeight = 25; // Height of each cell
                int cellWidth = 100; // Width of each cell
                int currentY = startY;

                // Print the column headers
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    string headerText = dataGridView1.Columns[i].HeaderText;
                    e.Graphics.DrawString(headerText, headerFont, brush, startX + (i * cellWidth), currentY);
                }

                currentY += cellHeight; // Move to the next row

                // Print the rows
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        string cellText = dataGridView1.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;
                        e.Graphics.DrawString(cellText, cellFont, brush, startX + (j * cellWidth), currentY);
                    }

                    currentY += cellHeight; // Move to the next row

                    // Check if the page is full
                    if (currentY > e.MarginBounds.Height)
                    {
                        e.HasMorePages = true; // Signal that another page is needed
                        return;
                    }
                }

                // All rows printed
                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while printing the page: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private string GetTableNameForIdColumn(string idColumn)
        {
            return idColumn switch
            {
                "request_id" => "sk_assistance_requests",
                "event_id" => "events",
                "budget_id" => "budgets",
                "project_id" => "projects",
                "households_id" => "households",
                "residents_id" => "residents",
                _ => throw new ArgumentException("Invalid ID column.")
            };
        }

        /// <summary>
        /// Prompts the user to select the column to update.
        /// </summary>
        private string PromptForColumnToUpdate(DataGridViewRow row)
        {
            // Example: You can replace this with a custom dialog or dropdown list
            string[] columns = row.DataGridView.Columns.Cast<DataGridViewColumn>().Select(c => c.Name).ToArray();
            string columnToUpdate = Microsoft.VisualBasic.Interaction.InputBox("Enter the column name to update:", "Update Column", columns.FirstOrDefault());
            return columns.Contains(columnToUpdate) ? columnToUpdate : null;
        }

        /// <summary>
        /// Prompts the user to input the new value for the selected column.
        /// </summary>
        private string PromptForNewValue(string column)
        {
            // Example: You can replace this with a custom dialog for user input
            return Microsoft.VisualBasic.Interaction.InputBox($"Enter the new value for {column}:", "Update Value", "");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
