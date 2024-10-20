using System.Data.Entity;
using ClinicQueueManagement.Models;

namespace ClinicQueueManagement.Data
{
	public class ClinicDbContext : DbContext
	{
		public ClinicDbContext() : base("name=MyClinicDB")
		{
		}

		public DbSet<Appointment> Appointments { get; set; }
	}
}
