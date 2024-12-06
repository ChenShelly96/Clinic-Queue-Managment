using ClinicQueueManagement.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ClinicQueueManagement.ScreensModels
{
	public partial class WaitingRoomScreen2 : Window
	{
		private readonly QueueManager _queueManager;
		private readonly DispatcherTimer _refreshTimer;
		private DispatcherTimer _queueRefreshTimer; // Timer to refresh the queue every 10 seconds
		private DispatcherTimer _clockTimer;

		public WaitingRoomScreen2(QueueManager queueManager)
		{
			InitializeComponent();
			_queueManager = queueManager;

			// Set up the timer to refresh the queue every 10 seconds
			_queueRefreshTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(10) // Set interval to 10 seconds
			};
			_queueRefreshTimer.Tick += RefreshCards; // Add event handler for the timer tick
			_queueRefreshTimer.Start(); // Start the timer

			// Set up the timer for the digital clock
			_clockTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(1) // Set interval to 1 second
			};
			_clockTimer.Tick += ClockTimer_Tick; // Add event handler for the clock timer
			_clockTimer.Start(); // Start the clock timer

			// Initial refresh of cards
			RefreshCards(null, null);
		}
		// Event handler to refresh the appointments list every 10 seconds
		


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
		
		// Update the "Now" section with the next appointment for each room
		private void UpdateNowSection()
		{
			// Get the list of next appointments and group them by room number
			List<Appointment> nextAppointments = _queueManager.GetNextAppointments();

			// Group appointments by room number
			var groupedAppointments = nextAppointments
				.Where(a => !a.IsCompleted)
				.GroupBy(a => a.RoomNumber)
				.ToDictionary(g => g.Key, g => g.OrderBy(a => a.AppointmentTime).FirstOrDefault());

			// Update the "Now" section for each room
			string nowText = "";

			for (int roomNumber = 1; roomNumber <= 4; roomNumber++)
			{
				if (groupedAppointments.TryGetValue(roomNumber, out Appointment currentAppointment) && currentAppointment != null)
				{
					nowText += $" מספר {currentAppointment.AppointmentID} לחדר מספר  {roomNumber}  \n";
				}
				else
				{
					// Clear the "Now" section if no active appointments are available
					CurrentPatientTextBlock.Text = "";
				}

			}

			// Display the updated "Now" text
			CurrentPatientTextBlock.Text = nowText.TrimEnd();
		}
		private void RefreshCards(object sender, EventArgs e)
		{
			// Clear existing cards
			CardsContainer.Children.Clear();

			// Get the next appointments from the queue
			var appointments = _queueManager.GetNextAppointments();

			foreach (var appointment in appointments)
			{
				var card = CreateAppointmentCard(appointment);
				CardsContainer.Children.Add(card);
			}

			// Update the "Currently Serving" section
			var currentAppointment = _queueManager.GetNextAppointments().Count > 0
				? _queueManager.GetNextAppointments()[0]
				: null;

			CurrentPatientTextBlock.Text = currentAppointment != null
				? $"מספר {currentAppointment.AppointmentID} - חדר {currentAppointment.RoomNumber}"
				: "No Active Appointments";
		}

		private Border CreateAppointmentCard(Appointment appointment)
		{
			// Create card container
			var card = new Border
			{
				Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BBDEFB")),
				BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1565C0")),
				BorderThickness = new Thickness(2),
				CornerRadius = new CornerRadius(10),
				Padding = new Thickness(10),
				Margin = new Thickness(10),
				Width = 600,
				Height = 70
			};

			// Create the stack panel for card content
			var stackPanel = new StackPanel
			{
				Orientation = Orientation.Vertical,
				Margin = new Thickness(5)
			};

			// Add text elements
			stackPanel.Children.Add(new TextBlock
			{
				Text = $"מספר {appointment.AppointmentID} לחדר {appointment.RoomNumber}",
				TextAlignment = TextAlignment.Center,
				FontSize = 20,
				FontWeight = FontWeights.Bold,
				Foreground = Brushes.Black,
				Margin = new Thickness(5)
			});

			

			card.Child = stackPanel;
			return card;
		}
	}
}
