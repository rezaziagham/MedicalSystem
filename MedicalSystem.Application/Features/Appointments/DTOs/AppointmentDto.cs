using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Features.Appointments.DTOs
{
	public class AppointmentDto
	{
		public Guid AppointmentId { get; set; }
		public Guid DoctorId { get; set; }
		public Guid PatientId { get; set; }
		public Guid ScheduleId { get; set; }
		public int QueueNumber { get; set; }
		public string Status { get; set; }
		public string? Notes { get; set; }
	}

}
