using AutoMapper;
using MassTransit;
using MediatR;
using MedicalSystem.Application.Common.Interfaces;
using MedicalSystem.Application.Common.Messaging.Events;
using MedicalSystem.Application.Features.Appointments.DTOs;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Features.Appointments.Commands.Update
{
	public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentCommand, Unit>
	{
		private readonly IApplicationDbContext _context;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IMapper _mapper;
		public UpdateAppointmentHandler(IApplicationDbContext context, IPublishEndpoint publishEndpoint, IMapper mapper)
		{
			_context = context;
			_publishEndpoint = publishEndpoint;
			_mapper = mapper;
		}
		public async Task<Unit> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
		{
			var appointment = await _context.Appointments.FindAsync(new object[] { request.Id }, cancellationToken);

			if (appointment == null)
			{
				throw new NotFoundException("َAppointment not found");
			}

			// Map updated fields
			_mapper.Map(request, appointment);

			_context.Appointments.Update(appointment);
			await _context.SaveChangesAsync(cancellationToken);

			// Optional: publish an event (if you have an event class like AppointmentUpdated)
			await _publishEndpoint.Publish<IAppointmentUpdatedEvent>(new
			{
				appointment.AppointmentId,
				appointment.DoctorId,
				appointment.PatientId,
				CreatedAt = DateTime.UtcNow
			});

			return new Unit();
		}
	}
}
