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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            // Optional: Add code to handle label click, or leave empty if not needed
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add code to handle text change, or leave empty if not needed
        }

        private void Home_Load(object sender, EventArgs e)
        {
            // Optional: Add code to handle form load, or leave empty if not needed
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }

            string query = "SELECT COUNT(*) FROM sk_officials WHERE email = @email AND password = @password";
            var emailParam = new MySql.Data.MySqlClient.MySqlParameter("@email", email);
            var passwordParam = new MySql.Data.MySqlClient.MySqlParameter("@password", password);

            try
            {
                using (var reader = DatabaseHelper.ExecuteQuery(query, emailParam, passwordParam))
                {
                    if (reader.Read() && reader.GetInt32(0) > 0)
                    {
                        // Login successful, open Form1
                        Form1 form1 = new Form1();
                        form1.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid email or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login failed: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 forgetform = new Form3();
            forgetform.Show();
            this.Hide();
        }
    }
}
