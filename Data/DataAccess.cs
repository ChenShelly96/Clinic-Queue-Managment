using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ClinicQueueManagement.Data
{
	public class DataAccess
	{

		private const string ConnectionString = "Server=DESKTOP-CHEN\\MSSQLSERVER01;Database=MyClinicDB;Trusted_Connection=True";
		// Method to save an appointment to the database
		public void SaveAppointmentToDatabase(DateTime appointmentTime, int appointmentNumber, string clinic)
		{
			// Connection string for the SQL Server instance and database
			//string connectionString = "Server=DESKTOP-CHEN\\MSSQLSERVER01;Database=MyClinicDB;Trusted_Connection=True";

			// Open the connection to the database
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();

				// SQL query to insert the appointment into the Appointments table
				string query = "INSERT INTO Appointments (AppointmentID, PatientName, AppointmentTime, IsCompleted) VALUES (@AppointmentID, 'Patient', @AppointmentTime, 0)";

				// Prepare and execute the SQL command
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					// Add parameters to the command to avoid SQL injection
					command.Parameters.AddWithValue("@AppointmentID", appointmentNumber);
					command.Parameters.AddWithValue("@AppointmentTime", appointmentTime);

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
				using (SqlConnection connection = new SqlConnection(ConnectionString))
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
		}


	}

