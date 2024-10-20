using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicQueueManagement.Models
{
	public class Appointment
	{
		public int AppointmentID { get; set; }
		public string PatientName { get; set; }
		public DateTime AppointmentTime { get; set; }
		public bool IsCompleted { get; set; }
	}
}
