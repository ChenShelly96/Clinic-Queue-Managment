using ClinicQueueManagement.Data;
using ClinicQueueManagement.ScreensModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ClinicQueueManagement
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
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
			WaitingRoomScreen waitingRoomScreen = new WaitingRoomScreen(_queueManager);
			waitingRoomScreen.Show();
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
	}
}
