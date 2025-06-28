using MediatR;
using MedicalSystem.Application.Features.Appointments.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Features.Appointments.Queries
{
	public class GetAppointmentsQuery : IRequest<List<AppointmentDto>>
	{
		public Guid? AppointmentId { get; set; }
	}

}
