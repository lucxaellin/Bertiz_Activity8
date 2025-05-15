using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using WindowsFormsApp1;

namespace GuiForDentalA
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hash the entered password using HashingHelper
            string hashedPassword = HashingHelper.HashPassword(password);

            try
            {
                // Query to check if the user exists
                string query = "SELECT patient_id FROM patients WHERE email = @Username AND password = @Password";

                // Execute the query using DatabaseHelper
                using (var reader = DatabaseHelper.ExecuteQuery(query,
                    new MySqlParameter("@Username", username),
                    new MySqlParameter("@Password", hashedPassword)))
                {
                    if (reader.Read())
                    {
                        // Get the patient ID
                        int patientId = Convert.ToInt32(reader["patient_id"]);

                        // Set the current user in UserSession
                        UserSession.SetCurrentUser(patientId);

                        // Login successful
                        Form4 userform = new Form4();
                        this.Hide();
                        userform.Show();
                    }
                    else
                    {
                        // Login failed
                        MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 signupForm = new Form3();
            this.Hide();
            signupForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (username == "admin" && password == "admin123")
            {
                Form1 mainForm = new Form1();
                this.Hide();
                mainForm.Show();
            }
            else
            {
                MessageBox.Show("Invalid login credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form5 PasswordForm  = new Form5();
            this.Hide();
            PasswordForm.Show();
        }
    }
}
