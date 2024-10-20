using System;
using ClinicQueueManagement.Data;

namespace ClinicQueueManagement
{
	class Program
	{
		static void Main(string[] args)
		{
			// Create an instance of DatabaseTester
			DatabaseTester tester = new DatabaseTester();

			// Test the connection to the SQL Server
			if (tester.TestConnection())
			{
				// Insert sample data into the database
				tester.InsertSampleData();

				// Fetch and display the data from the database
				tester.FetchData();
			}

			// Wait for a key press to close the console window
			Console.Read();
		}
	}
}
