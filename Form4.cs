using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GuiForDentalA
{
    public partial class Form4 : Form
    {
        private Label welcomeLabel;
        private DataGridView appointmentsGridView;

        public Form4()
        {
            InitializeComponent();

            DatabaseHelper.TestDatabaseConnection(); // Optional: test connection
            InitializeWelcomeLabel();
            InitializeAppointmentsGrid();
            LoadAppointments();
        }

        // Display welcome message
        private void InitializeWelcomeLabel()
        {
            try
            {
                welcomeLabel = new Label
                {
                    AutoSize = false,
                    Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Bold),
                    Location = new Point(150, 20),
                    Name = "welcomeLabel",
                    Size = new Size(600, 60),
                    Text = $"Welcome, {UserSession.FirstName} {UserSession.LastName}!",
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.LightBlue,
                    ForeColor = Color.DarkBlue,
                    BorderStyle = BorderStyle.FixedSingle
                };
                this.Controls.Add(welcomeLabel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Configure the appointments grid
        private void InitializeAppointmentsGrid()
        {
        }
        // Load appointments for the current user
        private void LoadAppointments()
        {
            try
            {
                List<Appointment> appointments = AppointmentManager.GetAppointmentsForCurrentUser();
                appointmentsGridView.DataSource = appointments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load appointments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Placeholder button events
        // Add Appointment
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Sample hardcoded appointment - replace with form inputs or a dialog
                var newAppointment = new Appointment
                {
                    PatientId = UserSession.CurrentUserId,
                    DentistId = 1, // Example: hardcoded dentist ID
                    AppointmentDate = DateTime.Now.AddDays(3),
                    Status = "Scheduled",
                    Notes = "Initial consultation"
                };

                AppointmentManager.AddAppointment(newAppointment);
                MessageBox.Show("Appointment added.");
                LoadAppointments(); // Refresh
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding appointment: {ex.Message}");
            }
        }

        // Update Appointment
        private void button5_Click(object sender, EventArgs e)
        {
            if (appointmentsGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to update.");
                return;
            }

            try
            {
                var row = appointmentsGridView.SelectedRows[0];
                var appointment = (Appointment)row.DataBoundItem;

                // Example update: change the notes
                appointment.Notes = "Updated notes";
                AppointmentManager.UpdateAppointment(appointment);

                MessageBox.Show("Appointment updated.");
                LoadAppointments(); // Refresh
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating appointment: {ex.Message}");
            }
        }

        // Delete Appointment
        private void button1_Click(object sender, EventArgs e)
        {
            if (appointmentsGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to delete.");
                return;
            }

            try
            {
                var row = appointmentsGridView.SelectedRows[0];
                var appointment = (Appointment)row.DataBoundItem;

                var confirm = MessageBox.Show("Are you sure you want to delete this appointment?",
                                              "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    AppointmentManager.DeleteAppointment(appointment.AppointmentId);
                    MessageBox.Show("Appointment deleted.");
                    LoadAppointments(); // Refresh
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting appointment: {ex.Message}");
            }
        }
    }
}
