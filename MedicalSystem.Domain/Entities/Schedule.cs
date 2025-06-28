using MedicalSystem.Domain.Common;
using MedicalSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Domain.Entities
{
	public class Schedule : IAggregateRoot, IAuditableEntity
	{
		public Guid ScheduleId { get; set; }
		public Guid DoctorId { get; set; }
		public DayOfWeek DayOfWeek { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public int MaxAppointments { get; set; }
		public bool IsReviewed { get; set; }
		public ScheduleStatus Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Doctor Doctor { get; set; } = null!;
		public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
	}
}
