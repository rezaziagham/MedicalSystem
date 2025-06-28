using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Common.Messaging.Events
{
	public interface IAppointmentUpdatedEvent
	{
		Guid AppointmentId { get; }
		Guid DoctorId { get; }
		Guid PatientId { get; }
		DateTime CreatedAt { get; }
	}
}
