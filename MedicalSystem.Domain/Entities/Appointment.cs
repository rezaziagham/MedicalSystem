using MedicalSystem.Domain.Common;
using MedicalSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Domain.Entities
{
	public class Appointment : IAggregateRoot, IAuditableEntity, ISoftDeletable
	{
		public Guid AppointmentId { get; set; }
		public Guid DoctorId { get; set; }
		public Guid PatientId { get; set; }
		public Guid ScheduleId { get; set; }

		public int QueueNumber { get; set; }
		public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
		public string? Notes { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		public virtual Doctor Doctor { get; set; } = null!;
		public virtual Patient Patient { get; set; } = null!;
		public virtual Schedule Schedule { get; set; } = null!;
		public bool IsDeleted { get; set; } = false;
	}
}
