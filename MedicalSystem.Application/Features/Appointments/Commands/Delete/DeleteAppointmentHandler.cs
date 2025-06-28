using AutoMapper;
using MassTransit;
using MediatR;
using MedicalSystem.Application.Common.Interfaces;
using MedicalSystem.Application.Common.Messaging.Events;
using MedicalSystem.Application.Features.Appointments.Commands.Create;
using MedicalSystem.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Features.Appointments.Commands.Delete
{
	public class DeleteAppointmentHandler : IRequestHandler<DeleteAppointmentCommand, Guid>
	{
		private readonly IApplicationDbContext _context;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IMapper _mapper;

		public DeleteAppointmentHandler(IApplicationDbContext context, IPublishEndpoint publishEndpoint, IMapper mapper) // Injected
		{
			_context = context;
			_publishEndpoint = publishEndpoint;
			_mapper = mapper;
		}
		public async Task<Guid> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
		{
			var appointment = await _context.Appointments.FindAsync(new object[] { request.Id }, cancellationToken);
			if (appointment == null)
				throw new NotFoundException("Appointment not found.");

			_context.Appointments.Remove(appointment); // No need for await when removing
			await _context.SaveChangesAsync(cancellationToken);

			await _publishEndpoint.Publish<IAppointmentDeletedEvent>(new
			{
				appointment.AppointmentId,
				appointment.DoctorId,
				appointment.PatientId,
				CreatedAt = DateTime.UtcNow
			});

			return appointment.AppointmentId;
		}

	}
}
