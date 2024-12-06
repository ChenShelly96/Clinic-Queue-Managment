using ClinicQueueManagement.Data;
using ClinicQueueManagement.ScreensModels;
using System;
using System.Windows;
using System.Windows.Threading;

namespace ClinicQueueManagement
{
	public partial class MainWindow : Window
	{
		private QueueManager _queueManager;
		private DispatcherTimer _dailyCleanupTimer;
		private DataAccess _dataAccess;

		public MainWindow()
		{
			InitializeComponent();
			_queueManager = new QueueManager();

			// Schedule daily cleanup at 21:00
			_dailyCleanupTimer = new DispatcherTimer();
			_dailyCleanupTimer.Interval = GetTimeUntilCleanup();  // Set initial interval
			_dailyCleanupTimer.Tick += DailyCleanupTimer_Tick;    // Attach event handler
			_dailyCleanupTimer.Start();  // Start the timer
		}

		private void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
		{
			AddAppointmentWindow addAppointmentWindow = new AddAppointmentWindow();
			addAppointmentWindow.Show();
		}

		// Opens the Waiting Room Screen
		private void OpenWaitingRoomScreenButton_Click(object sender, RoutedEventArgs e)
		{
			// Open the Waiting Room Screen, passing the queue manager to display the queue
			//WaitingRoomScreen waitingRoomScreen = new WaitingRoomScreen(_queueManager);
			//waitingRoomScreen.Show();


			WaitingRoomScreen2 w2 = new WaitingRoomScreen2(_queueManager);
			w2.Show();
		}

		// Calculate the time remaining until 21:00 today
		private TimeSpan GetTimeUntilCleanup()
		{
			DateTime now = DateTime.Now;
			DateTime cleanupTime = new DateTime(now.Year, now.Month, now.Day, 21, 0, 0);
			if (now > cleanupTime)
			{
				// If it's past 21:00, set for tomorrow
				cleanupTime = cleanupTime.AddDays(1);
			}
			return cleanupTime - now;
		}

		// Event handler to perform cleanup at 21:00
		private void DailyCleanupTimer_Tick(object sender, EventArgs e)
		{
			// Delete all appointments from the database
			_dataAccess.ClearAppointmentsTable();

			// Reset the timer for the next day
			_dailyCleanupTimer.Interval = TimeSpan.FromHours(24);
		}

		private void OpenClinicRoom1Button_Click(object sender, RoutedEventArgs e)
		{
			// Pass the room number to the ClinicRoomScreen
			//int roomNumber = 1;  // For example, room 1
			//ClinicRoomScreen clinicRoomScreen = new ClinicRoomScreen(_queueManager, roomNumber);
			//clinicRoomScreen.Show();
			OpenClinicRoomScreen(1);
		}

		private void OpenClinicRoom2Button_Click(object sender, RoutedEventArgs e)
		{
			OpenClinicRoomScreen(2);
		}

		private void OpenClinicRoom3Button_Click(object sender, RoutedEventArgs e)
		{
			OpenClinicRoomScreen(3);
		}

		private void OpenClinicRoom4Button_Click(object sender, RoutedEventArgs e)
		{
			OpenClinicRoomScreen(4);
		}


		private void OpenClinicRoomScreen(int roomNumber)
		{
			ClinicRoomScreen clinicRoomScreen = new ClinicRoomScreen(_queueManager, roomNumber);
			clinicRoomScreen.Show();
		}
	}
}
