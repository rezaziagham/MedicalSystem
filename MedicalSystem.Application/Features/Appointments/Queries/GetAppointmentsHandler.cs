using MediatR;
using MedicalSystem.Application.Common.Interfaces;
using MedicalSystem.Application.Features.Appointments.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Features.Appointments.Queries
{
	public class GetAppointmentsHandler : IRequestHandler<GetAppointmentsQuery, List<AppointmentDto>>
	{
		private readonly IApplicationDbContext _context;

		public GetAppointmentsHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<AppointmentDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
		{
			var query = _context.Appointments.AsQueryable();

			if (request.AppointmentId.HasValue)
			{
				query = query.Where(x => x.AppointmentId == request.AppointmentId.Value && x.IsDeleted == false);
			}

			return await query.Select(a => new AppointmentDto
			{
				AppointmentId = a.AppointmentId,
				DoctorId = a.DoctorId,
				PatientId = a.PatientId,
				ScheduleId = a.ScheduleId,
				QueueNumber = a.QueueNumber,
				Status = a.Status.ToString(),
				Notes = a.Notes
			}).ToListAsync(cancellationToken);
		}
	}

}
