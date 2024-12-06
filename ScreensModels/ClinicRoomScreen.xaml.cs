using ClinicQueueManagement.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace ClinicQueueManagement.ScreensModels
{
	public partial class ClinicRoomScreen : Window
	{
		private QueueManager _queueManager;
		private Appointment _currentAppointment;
		private DispatcherTimer _autoRefreshTimer; // Timer for automatic refresh
		private int _roomNumber; // Room number associated with this screen

		public ClinicRoomScreen(QueueManager queueManager, int roomNumber)
		{
			InitializeComponent();
			_queueManager = queueManager;
			_roomNumber = roomNumber;

			LoadAppointmentsForRoom(roomNumber);

			// Subscribe to the AppointmentsUpdated event in QueueManager
			_queueManager.AppointmentsUpdated += OnAppointmentsUpdated;

			// Set up an auto-refresh timer to refresh the data every few seconds
			SetAutoRefreshTimer();
		}


		// Set up a timer for automatic refresh
		private void SetAutoRefreshTimer()
		{
			_autoRefreshTimer = new DispatcherTimer();
			_autoRefreshTimer.Interval = TimeSpan.FromSeconds(5); // Refresh every 5 seconds
			_autoRefreshTimer.Tick += (sender, e) => LoadAppointmentsForRoom(_roomNumber);
			_autoRefreshTimer.Start();
		}

		// Event handler for AppointmentsUpdated event
		private void OnAppointmentsUpdated()
		{
			// Refresh appointments for the current room
			Dispatcher.Invoke(() => LoadAppointmentsForRoom(_roomNumber));
		}
		// Load appointments for the specific room from the database and bind to the DataGrid
		private void LoadAppointmentsForRoom(int roomNumber)
		{
			List<Appointment> roomAppointments = _queueManager.GetAppointmentsForRoom(roomNumber);

			// Bind the queue to the DataGrid
			RoomQueueDataGrid.ItemsSource = roomAppointments;

			// Display the current appointment at the top of the queue
			_currentAppointment = roomAppointments.Count > 0 ? roomAppointments[0] : null;
			if (_currentAppointment != null)
			{
				CurrentAppointmentTextBlock.Text = $"#{_currentAppointment.AppointmentID} - {_currentAppointment.PatientName}";
				IsCompletedCheckBox.IsChecked = _currentAppointment.IsCompleted;
			}
		}

		// Event handler for when the Next Patient button is clicked
		private void NextPatientButton_Click(object sender, RoutedEventArgs e)
		{
			if (_currentAppointment != null && IsCompletedCheckBox.IsChecked == true)
			{
				// Mark the appointment as completed and remove it from the queue and database
				_queueManager.MarkAppointmentAsCompleted(_currentAppointment);
				LoadAppointmentsForRoom(_currentAppointment.RoomNumber);  // Reload the updated queue
			}
			else
			{
				MessageBox.Show("Please mark the current patient as completed before moving to the next.");
			}
		}
	}
}
