using System;
using System.Collections.Generic;
using System.Threading;
using ClinicQueueManagement.Data;

namespace ClinicQueueManagement.Data
{
	// Class to manage patient queues using a Min-Heap (Priority Queue)
	public class QueueManager
	{
		private List<Appointment> _appointmentsHeap; // List to represent the Min-Heap
		private DataAccess _dataAccess; // Object to interact with the database
		private Timer _timer; // Timer for updating the appointments every 20 seconds

		public QueueManager()
		{
			_appointmentsHeap = new List<Appointment>();
			_dataAccess = new DataAccess(); // Initialize DataAccess object

			// Load appointments from the database for today when initializing the QueueManager
			LoadAppointmentsFromDatabaseForToday();

			// Set up the timer to update appointments every 20 seconds (20,000 milliseconds)
			_timer = new Timer(UpdateAppointments, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
		}

		// Timer callback method to refresh appointments every 20 seconds
		private void UpdateAppointments(object state)
		{
			LoadAppointmentsFromDatabaseForToday();  // Load the updated appointments from the database
		}

		// Method to add an appointment to the Min-Heap
		public void AddAppointment(Appointment appointment)
		{
			_appointmentsHeap.Add(appointment);
			HeapifyUp(_appointmentsHeap.Count - 1); // Ensure the Min-Heap property is maintained
		}

		// Method to retrieve the next 20 appointments from the Min-Heap, sorted by time
		public List<Appointment> GetNextAppointments()
		{
			List<Appointment> sortedAppointments = new List<Appointment>(_appointmentsHeap);
			sortedAppointments.Sort((a, b) => a.AppointmentTime.CompareTo(b.AppointmentTime));  // Sort by appointment time
			return sortedAppointments.Count > 20 ? sortedAppointments.GetRange(0, 20) : sortedAppointments;
		}

		// Method to remove the appointment with the earliest time (Root of the heap)
		public Appointment RemoveEarliestAppointment()
		{
			if (_appointmentsHeap.Count == 0) return null;

			// Swap the root with the last element, remove the last, and heapify down
			Appointment earliest = _appointmentsHeap[0];
			_appointmentsHeap[0] = _appointmentsHeap[_appointmentsHeap.Count - 1];
			_appointmentsHeap.RemoveAt(_appointmentsHeap.Count - 1);
			HeapifyDown(0);

			return earliest;
		}

		// Method to load appointments for the current day from the database
		public void LoadAppointmentsFromDatabaseForToday()
		{
			// Clear the current heap
			_appointmentsHeap.Clear();

			// Fetch the appointments for today from the database
			List<Appointment> appointments = _dataAccess.LoadAppointmentsFromDatabaseForToday();

			// Add each appointment to the Min-Heap
			foreach (var appointment in appointments)
			{
				AddAppointment(appointment);
			}
		}

		// Method to maintain the Min-Heap property after adding an element
		private void HeapifyUp(int index)
		{
			while (index > 0)
			{
				int parentIndex = (index - 1) / 2;
				if (_appointmentsHeap[index].AppointmentTime < _appointmentsHeap[parentIndex].AppointmentTime)
				{
					Swap(index, parentIndex);
					index = parentIndex;
				}
				else
				{
					break;
				}
			}
		}

		// Method to maintain the Min-Heap property after removing an element
		private void HeapifyDown(int index)
		{
			int lastIndex = _appointmentsHeap.Count - 1;

			while (index < lastIndex)
			{
				int leftChild = 2 * index + 1;
				int rightChild = 2 * index + 2;
				int smallest = index;

				if (leftChild <= lastIndex && _appointmentsHeap[leftChild].AppointmentTime < _appointmentsHeap[smallest].AppointmentTime)
				{
					smallest = leftChild;
				}

				if (rightChild <= lastIndex && _appointmentsHeap[rightChild].AppointmentTime < _appointmentsHeap[smallest].AppointmentTime)
				{
					smallest = rightChild;
				}

				if (smallest != index)
				{
					Swap(index, smallest);
					index = smallest;
				}
				else
				{
					break;
				}
			}
		}

		// Helper method to swap two elements in the heap
		private void Swap(int index1, int index2)
		{
			var temp = _appointmentsHeap[index1];
			_appointmentsHeap[index1] = _appointmentsHeap[index2];
			_appointmentsHeap[index2] = temp;
		}
	}

	// Class representing an appointment
	public class Appointment
	{
		public int AppointmentID { get; set; }
		public string PatientName { get; set; }
		public DateTime AppointmentTime { get; set; }
		public bool IsCompleted { get; set; }
	}
}
