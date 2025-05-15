using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkProjectEdP
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //username
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //password
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //confirm password 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();
            string newPassword = textBox3.Text;
            string confirmPassword = textBox2.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            string query = "UPDATE sk_officials SET password = @password WHERE email = @email";
            var emailParam = new MySql.Data.MySqlClient.MySqlParameter("@email", email);
            var passwordParam = new MySql.Data.MySqlClient.MySqlParameter("@password", newPassword);

            try
            {
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, passwordParam, emailParam);
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Password has been updated successfully.");
                    // Redirect to Home form
                    Home homeForm = new Home();
                    homeForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("No user found with that email.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating password: {ex.Message}");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
