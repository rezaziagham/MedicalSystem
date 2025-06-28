using MediatR;
using MedicalSystem.Application.Features.Appointments.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Features.Appointments.Commands.Update
{
	public class UpdateAppointmentCommand : IRequest<Unit>
	{
		public Guid Id { get; set; }
		public AppointmentDto Appointment { get; set; }
	}
}
