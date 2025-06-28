using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Features.Appointments.Commands.Delete
{
	public class DeleteAppointmentCommand : IRequest<Guid>
	{
		public Guid Id { get; set; }
		public DeleteAppointmentCommand(Guid id)
		{
			Id = id;
		}
	}
}
