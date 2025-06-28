using AutoMapper;
using MassTransit;
using MediatR;
using MedicalSystem.Application.Common.Interfaces;
using MedicalSystem.Application.Common.Messaging.Events;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;
using MedicalSystem.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Features.Appointments.Commands.Create
{
	public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, Guid>
	{
		private readonly IApplicationDbContext _context;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IMapper _mapper;

		public CreateAppointmentHandler(IApplicationDbContext context,IPublishEndpoint publishEndpoint , IMapper mapper) // Injected
		{
			_context = context;
			_publishEndpoint = publishEndpoint;
			_mapper=mapper;
		}

		public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
		{
			// ✅ Fetch the schedule
			var schedule = await _context.Schedules
				.FirstOrDefaultAsync(s => s.ScheduleId == request.ScheduleId);

			if (schedule == null)
			{
				throw new NotFoundException("Schedule not found.");
			}

			// ✅ Check appointment limit (if applicable)
			if (schedule.MaxAppointments > 0)
			{
				var existingAppointments = _context.Appointments
					.Where(a => a.ScheduleId == request.ScheduleId);

				if (await existingAppointments.CountAsync() >= schedule.MaxAppointments)
				{
					throw new InvalidOperationException("Maximum appointments reached for this schedule.");
				}
			}

			// ✅ Count current appointments to assign QueueNumber
			var currentAppointments = _context.Appointments
				.Where(a => a.ScheduleId == request.ScheduleId);
			int queueNumber = await currentAppointments.CountAsync() + 1;

			// ✅ Map and create appointment
			var appointment = _mapper.Map<Appointment>(request);
			appointment.AppointmentId = Guid.NewGuid();
			appointment.Status = AppointmentStatus.Pending;
			appointment.QueueNumber = queueNumber;
			appointment.DoctorId = schedule.DoctorId;

			await _context.Appointments.AddAsync(appointment, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			await _publishEndpoint.Publish<IAppointmentCreatedEvent>(new
			{
				appointment.AppointmentId,
				appointment.DoctorId,
				appointment.PatientId,
				CreatedAt = DateTime.UtcNow
			});

			// ✅ Return the created appointment ID
			return appointment.AppointmentId;
		}
	}

}
