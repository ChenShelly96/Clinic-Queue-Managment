using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ClinicQueueManagement.Data;

namespace ClinicQueueManagement
{
	public partial class AddAppointmentWindow : Window
	{
		private DataAccess _dataAccess;  // Handles database operations
		private QueueManager _queueManager;  // Handles the Min-Heap logic for appointments

		public AddAppointmentWindow()
		{
			InitializeComponent();
			_dataAccess = new DataAccess();  // Initialize DataAccess object for database interaction
			_queueManager = new QueueManager();  // Initialize QueueManager for managing appointments
		}

		private void AppointmentTimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			// TextBox logic can be added here if needed
		}

		// Clinic 1 Button click handler
		private void Clinic1Button_Click(object sender, RoutedEventArgs e)
		{
			string inputTime = AppointmentTimeTextBox.Text;

			// Validate the input time format (valid times are between 07:00 and 23:59)
			if (ValidateTime(inputTime))
			{
				// Parse the valid time input
				DateTime appointmentTime = DateTime.Parse(inputTime);

				// Generate a unique appointment number (between 100 and 1000)
				int appointmentNumber = GenerateAppointmentNumber();

				// Create a new appointment object
				Appointment newAppointment = new Appointment
				{
					AppointmentID = appointmentNumber,
					PatientName = "Patient",
					AppointmentTime = appointmentTime,
					IsCompleted = false
				};

				// Add the appointment to the Min-Heap (priority queue) managed by QueueManager
				_queueManager.AddAppointment(newAppointment);

				// Save the appointment to the database (via DataAccess) after it is queued
				_dataAccess.SaveAppointmentToDatabase(appointmentTime, appointmentNumber, "Clinic 1");

				// Display confirmation message
				MessageBox.Show($"Appointment created for Clinic 1 with number: {appointmentNumber} at {appointmentTime}");
			}
			else
			{
				// Show an error message if the time input is invalid
				MessageBox.Show("Invalid time format. Please enter a time between 07:00 and 23:59.");
			}
		}

		// Validates the time input format using regular expressions
		// Ensures the time is between 07:00 and 23:59 in HH:mm format
		private bool ValidateTime(string time)
		{
			// Regular expression to match time in HH:mm format, only between 07:00 and 23:59
			Regex timeFormat = new Regex(@"^(0[7-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
			return timeFormat.IsMatch(time);
		}

		// Generates a unique appointment number between 100 and 1000
		// Used for assigning a unique identifier to each appointment
		private int GenerateAppointmentNumber()
		{
			Random random = new Random();
			return random.Next(100, 1001);
		}
	}
}
