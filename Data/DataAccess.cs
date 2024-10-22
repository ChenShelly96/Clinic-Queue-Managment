using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;



namespace ClinicQueueManagement.Data
{
	public class DataAccess
	{

		//private const string ConnectionString = "Server=DESKTOP-CHEN\\MSSQLSERVER01;Database=MyClinicDB;Trusted_Connection=True";
		private readonly string _connectionString;


		public DataAccess()
		{
			// Load the connection string from appsettings.json
			var config = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)  // Set base path to where the appsettings.json is located
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // Add appsettings.json
				.Build();

			_connectionString = config.GetConnectionString("DefaultConnection");
		}


		// Method to save an appointment to the database
		public void SaveAppointmentToDatabase(DateTime appointmentTime, int appointmentNumber, int roomNumber)
		{
			// Connection string for the SQL Server instance and database
			//string connectionString = "Server=DESKTOP-CHEN\\MSSQLSERVER01;Database=MyClinicDB;Trusted_Connection=True";

			// Open the connection to the database
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				// SQL query to insert the appointment into the Appointments table
				string query = "INSERT INTO Appointments (AppointmentID, PatientName, AppointmentTime, IsCompleted, RoomNumber)" +
					" VALUES (@AppointmentID, 'Patient', @AppointmentTime,0, @RoomNumber)";

				// Prepare and execute the SQL command
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					// Add parameters to the command to avoid SQL injection
					command.Parameters.AddWithValue("@AppointmentID", appointmentNumber);
					command.Parameters.AddWithValue("@AppointmentTime", appointmentTime);
					command.Parameters.AddWithValue("@RoomNumber", roomNumber);
					// Execute the command
					command.ExecuteNonQuery();
				}
			}
		}

		// Method to load appointments for the current day from the database
		public List<Appointment> LoadAppointmentsFromDatabaseForToday()
		{
			List<Appointment> appointments = new List<Appointment>();

			// Use the constant ConnectionString
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				// SQL query to select appointments for today
				string query = "SELECT AppointmentID, PatientName, AppointmentTime, IsCompleted FROM Appointments WHERE CAST(AppointmentTime AS DATE) = CAST(GETDATE() AS DATE)";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							Appointment appointment = new Appointment
							{
								AppointmentID = (int)reader["AppointmentID"],
								PatientName = (string)reader["PatientName"],
								AppointmentTime = (DateTime)reader["AppointmentTime"],
								IsCompleted = (bool)reader["IsCompleted"]
							};

							appointments.Add(appointment); // Add each appointment to the list
						}
					}
				}
			}

			return appointments;
		}


		// Method to mark an appointment as completed in the database
		public void MarkAppointmentCompletedInDatabase(int appointmentID)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "UPDATE Appointments SET IsCompleted = 1 WHERE AppointmentID = @AppointmentID";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@AppointmentID", appointmentID);
					command.ExecuteNonQuery();  // Execute the update query
				}
			}
		}

		public void ClearAppointmentsTable()
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "DELETE FROM Appointments";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.ExecuteNonQuery();
				}
			}
		}

		// Method to remove an appointment from the database
		public static void RemoveAppointmentFromDatabase(int appointmentId)
		{
			string connectionString = "your_connection_string_here";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				string query = "DELETE FROM Appointments WHERE AppointmentID = @AppointmentID";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@AppointmentID", appointmentId);
					command.ExecuteNonQuery();
				}
			}
		}

	}


}



