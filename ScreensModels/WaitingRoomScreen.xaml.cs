using ClinicQueueManagement.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;

namespace ClinicQueueManagement.ScreensModels
{
	public partial class WaitingRoomScreen : Window
	{
		private QueueManager _queueManager;
		private DispatcherTimer _queueRefreshTimer; // Timer to refresh the queue every 10 seconds
		private DispatcherTimer _clockTimer; // Timer to update the clock every second

		public WaitingRoomScreen(QueueManager queueManager)
		{
			InitializeComponent();
			_queueManager = queueManager;

			// Load appointments initially
			LoadAppointments();

			// Set up the timer to refresh the queue every 10 seconds
			_queueRefreshTimer = new DispatcherTimer();
			_queueRefreshTimer.Interval = TimeSpan.FromSeconds(10); // Set interval to 10 seconds
			_queueRefreshTimer.Tick += QueueRefreshTimer_Tick; // Add event handler for the timer tick
			_queueRefreshTimer.Start(); // Start the timer

			// Set up the timer for the digital clock
			_clockTimer = new DispatcherTimer();
			_clockTimer.Interval = TimeSpan.FromSeconds(1); // Set interval to 1 second
			_clockTimer.Tick += ClockTimer_Tick; // Add event handler for the clock timer
			_clockTimer.Start(); // Start the clock timer
		}

		// Event handler to refresh the appointments list every 10 seconds
		private void QueueRefreshTimer_Tick(object sender, EventArgs e)
		{
			LoadAppointments(); // Refresh the appointment list
			UpdateNowSection(); // Update the "Now" section with the current patient
		}

		// Event handler to update the date, day of the week, and clock every second
		private void ClockTimer_Tick(object sender, EventArgs e)
		{
			// Display the current date
			DateTextBlock.Text = DateTime.Now.ToString("dd/MM/yyyy");

			// Display the current day of the week in Hebrew
			DayTextBlock.Text = DateTime.Now.ToString("dddd", new CultureInfo("he-IL"));

			// Display the current time in HH:mm format
			ClockTextBlock.Text = DateTime.Now.ToString("HH:mm");
		}

		// Load the next 20 appointments from the Min-Heap (priority queue) and display them per clinic
		private void LoadAppointments()
		{


			// Clear existing items in the Clinic 1 DataGrid
			Clinic1DataGrid.ItemsSource = null;

			// Load appointments and sort them by clinic
			List<Appointment> nextAppointments = _queueManager.GetNextAppointments();

			// Bind the appointment list to the DataGrid
			Clinic1DataGrid.ItemsSource = nextAppointments;

			
		}

		// Refresh button to reload the queue manually
		private void RefreshQueue_Click(object sender, RoutedEventArgs e)
		{
			LoadAppointments();
		}

		// Update the "Now" section with the next appointment to be called
		private void UpdateNowSection()
		{
			Appointment nextAppointment = _queueManager.RemoveEarliestAppointment();

			if (nextAppointment != null)
			{
				// Assuming that appointments with ID in a specific range belong to Clinic 1 or Clinic 2
				string clinicName = (nextAppointment.AppointmentID % 2 == 0) ? "1" : "2";

				// Update the NowQueueTextBlock with the patient's number and the clinic
				NowQueueTextBlock.Text = $"מספר {nextAppointment.AppointmentID} לחדר מספר {nextAppointment.RoomNumber}";
			}
		}
	}
}
