using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Features.Appointments.Commands.Create
{
	public class CreateAppointmentCommand : IRequest<Guid>
	{
		public Guid PatientId { get; set; }
		public Guid ScheduleId { get; set; }
		public string? Notes { get; set; }
	}
}
