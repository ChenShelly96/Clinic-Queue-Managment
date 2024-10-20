using ClinicQueueManagement.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace ClinicQueueManagement.ScreensModels
{
	public partial class WaitingRoomScreen : Window
	{
		private QueueManager _queueManager;
		private DispatcherTimer _timer; // Timer to refresh the queue every 10 seconds

		public WaitingRoomScreen(QueueManager queueManager)
		{
			InitializeComponent();
			_queueManager = queueManager;

			// Load appointments initially
			LoadAppointments();

			// Set up the timer to refresh the queue every 10 seconds
			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromSeconds(10); // Set interval to 10 seconds
			_timer.Tick += Timer_Tick; // Add event handler for the timer tick
			_timer.Start(); // Start the timer
		}

		// Event handler that is called every 10 seconds to refresh the appointments list
		private void Timer_Tick(object sender, EventArgs e)
		{
			LoadAppointments(); // Refresh the appointment list
		}

		// Load the next 20 appointments from the Min-Heap (priority queue) and display them per clinic
		private void LoadAppointments()
		{
			// Clear existing items in both clinics
			Clinic1ListBox.Items.Clear();
			//Clinic2ListBox.Items.Clear();

			// Load appointments and sort them by clinic
			List<Appointment> nextAppointments = _queueManager.GetNextAppointments();

			foreach (var appointment in nextAppointments)
			{
				
					Clinic1ListBox.Items.Add($"Appointment ID: {appointment.AppointmentID}");
				
				
			}
		}

		// Refresh button to reload the queue manually
		private void RefreshQueue_Click(object sender, RoutedEventArgs e)
		{
			LoadAppointments();
		}
	}
}
