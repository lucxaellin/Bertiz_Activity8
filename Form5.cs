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

namespace GuiForDentalA
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim(); 
            string newPassword = textBox2.Text.Trim();
            string confirmPassword = textBox3.Text.Trim();

            
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please complete all fields.");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                // Verify if the email exists in the database
                string checkQuery = "SELECT patient_id FROM patients WHERE email = @Email";
                MySqlParameter[] checkParams = {
                    new MySqlParameter("@Email", email)
                };

                using (MySqlDataReader reader = DatabaseHelper.ExecuteQuery(checkQuery, checkParams))
                {
                    if (reader.Read())
                    {
                        reader.Close(); // Close before reusing the connection

                        // Generate a hashed version of the new password
                        string hashedPassword = HashingHelper.HashPassword(newPassword);

                        // Update the password in the database
                        string updateQuery = "UPDATE patients SET password = @NewPassword WHERE email = @Email";
                        MySqlParameter[] updateParams = {
                            new MySqlParameter("@NewPassword", hashedPassword),
                            new MySqlParameter("@Email", email)
                        };

                        int rowsAffected = DatabaseHelper.ExecuteNonQuery(updateQuery, updateParams);

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password successfully updated.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to update password. Please try again.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Email not found. Please check the email address or register.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }






        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
