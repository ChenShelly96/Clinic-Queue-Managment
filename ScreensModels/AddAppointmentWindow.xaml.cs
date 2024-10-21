using System;
using System.Windows;
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

		// Common logic to handle adding appointments for any room
		private void AddAppointment(int roomNumber)
		{
			string inputTime = AppointmentTimeTextBox.Text;

			if (ValidateTime(inputTime))  // Validate the time input
			{
				DateTime appointmentTime = DateTime.Parse(inputTime);
				
				int appointmentNumber = CalculateAppointmentNumber(appointmentTime);
				// Create a new appointment object
				Appointment newAppointment = new Appointment
				{
					AppointmentID = appointmentNumber,
					PatientName = "Patient",  // This could be customized
					AppointmentTime = appointmentTime,
					RoomNumber = roomNumber,  // Assign selected room
					IsCompleted = false
				};

				// Add to the Min-Heap (priority queue)
				_queueManager.AddAppointment(newAppointment);

				// Save the appointment to the database
				_dataAccess.SaveAppointmentToDatabase(appointmentTime, appointmentNumber, roomNumber);

				MessageBox.Show($"Appointment created for Room {roomNumber} with number: {appointmentNumber} at {appointmentTime}");
			}
			else
			{
				MessageBox.Show("Invalid time format. Please enter a time in HH:mm format.");
			}
		}

		// Button click handlers for each room
		private void Clinic1Button_Click(object sender, RoutedEventArgs e)
		{
			AddAppointment(1);  // Room 1
		}

		private void Clinic2Button_Click(object sender, RoutedEventArgs e)
		{
			AddAppointment(2);  // Room 2
		}

		private void Clinic3Button_Click(object sender, RoutedEventArgs e)
		{
			AddAppointment(3);  // Room 3
		}

		private void Clinic4Button_Click(object sender, RoutedEventArgs e)
		{
			AddAppointment(4);  // Room 4
		}

		// Method to validate the input time
		private bool ValidateTime(string time)
		{
			return DateTime.TryParse(time, out _);  // Simple validation, could be extended
		}

	

		// Method to calculate appointment number based on time
		private int CalculateAppointmentNumber(DateTime appointmentTime)
		{
			DateTime startOfDay = new DateTime(appointmentTime.Year, appointmentTime.Month, appointmentTime.Day, 8, 0, 0); // Start at 08:00 AM
			TimeSpan timeDifference = appointmentTime - startOfDay;
			int appointmentNumber = 100 + (int)(timeDifference.TotalMinutes / 5);  // Increment by 1 every 5 minutes
			return appointmentNumber;
		}
	}
}
