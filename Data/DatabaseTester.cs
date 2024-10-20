using System;
using System.Data.SqlClient;
using System.Windows;

namespace ClinicQueueManagement.Data
{
	public class DatabaseTester
	{
		// Connection string for the SQL Server instance and database
		 string connectionString = "Server=DESKTOP-CHEN\\MSSQLSERVER01;Database=MyClinicDB;Trusted_Connection=True";

		// Method to test the connection to the SQL Server
		public bool TestConnection()
		{

			SqlConnection con = new SqlConnection(connectionString);
			con.Open();
			string query = "Select * from dbo.Appointments";
			SqlCommand cmd = new SqlCommand(query, con);
			SqlDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				string output = "Output = " + reader.GetValue(0) + "-" + reader.GetValue(1) + "-" + reader.GetValue(2);
				//MessageBox.Show(output);
				Console.WriteLine(output);
				return true;
			}
			return false;
			/*try
			{
				using (SqlConnection connection = new SqlConnection("Server=MSSQLSERVER01;Database=MyClinicDB;Trusted_Connection=True"))
				{
					connection.Open();
					Console.WriteLine("Connected to SQL Server successfully.");
					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Connection failed: " + ex.Message);
				return false;
			}*/
		}

		// Method to insert sample data into the 'Appointments' table
		public void InsertSampleData()
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					// Insert query to add a sample appointment
					string insertQuery = "INSERT INTO Appointments (PatientName, AppointmentTime, IsCompleted) " +
										 "VALUES (@PatientName, @AppointmentTime, @IsCompleted)";

					using (SqlCommand command = new SqlCommand(insertQuery, connection))
					{
						// Add parameters to avoid SQL injection
						command.Parameters.AddWithValue("@PatientName", "John Doe");
						command.Parameters.AddWithValue("@AppointmentTime", DateTime.Now.AddHours(2));
						command.Parameters.AddWithValue("@IsCompleted", false);

						// Execute the insert query and show how many rows were affected
						int rowsAffected = command.ExecuteNonQuery();
						Console.WriteLine($"{rowsAffected} row(s) inserted successfully.");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Insert failed: " + ex.Message);
			}
		}

		// Method to fetch and display data from the 'Appointments' table
		public void FetchData()
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					// Select query to fetch all appointments
					string selectQuery = "SELECT AppointmentID, PatientName, AppointmentTime, IsCompleted FROM Appointments";

					using (SqlCommand command = new SqlCommand(selectQuery, connection))
					{
						// Read and display the results of the query
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								Console.WriteLine($"ID: {reader["AppointmentID"]}, Name: {reader["PatientName"]}, " +
												  $"Time: {reader["AppointmentTime"]}, Completed: {reader["IsCompleted"]}");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Fetch failed: " + ex.Message);
			}
		}
	}
}
