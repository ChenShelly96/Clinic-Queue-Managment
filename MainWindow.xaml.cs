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

namespace ClinicQueueManagement
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private QueueManager _queueManager;
		public MainWindow()
		{
			InitializeComponent();
			_queueManager = new QueueManager();
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
	}
}
