using MedicalSystem.Application.Features.Appointments.Queries;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;
using MedicalSystem.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Tests.Application.Appointments
{
	public class GetAppointmentsQueryTests : TestBase
	{
		[Fact]
		public async Task Should_Return_All_Appointments()
		{
			var appointment1 = new Appointment
			{
				AppointmentId = Guid.NewGuid(),
				DoctorId = Guid.NewGuid(),
				PatientId = Guid.NewGuid(),
				ScheduleId = Guid.NewGuid(),
				QueueNumber = 1,
				Status = AppointmentStatus.Pending,
				Notes = "First"
			};
			var appointment2 = new Appointment
			{
				AppointmentId = Guid.NewGuid(),
				DoctorId = Guid.NewGuid(),
				PatientId = Guid.NewGuid(),
				ScheduleId = Guid.NewGuid(),
				QueueNumber = 2,
				Status = AppointmentStatus.Completed,
				Notes = "Second"
			};
			await _dbContext.Appointments.AddRangeAsync(appointment1, appointment2);
			await _dbContext.SaveChangesAsync();

			var query = new GetAppointmentsQuery();

			var handler = new GetAppointmentsHandler(_dbContext);

			var result = await handler.Handle(query, CancellationToken.None);

			Assert.Equal(2, result.Count);
		}

		[Fact]
		public async Task Should_Return_Specific_Appointment_ById()
		{
			var appointment = new Appointment
			{
				AppointmentId = Guid.NewGuid(),
				DoctorId = Guid.NewGuid(),
				PatientId = Guid.NewGuid(),
				ScheduleId = Guid.NewGuid(),
				QueueNumber = 1,
				Status = AppointmentStatus.Pending,
				Notes = "Target"
			};
			await _dbContext.Appointments.AddAsync(appointment);
			await _dbContext.SaveChangesAsync();

			var query = new GetAppointmentsQuery
			{
				AppointmentId = appointment.AppointmentId
			};

			var handler = new GetAppointmentsHandler(_dbContext);

			var result = await handler.Handle(query, CancellationToken.None);

			Assert.Single(result);
			Assert.Equal(appointment.Notes, result[0].Notes);
		}

		[Fact]
		public async Task Should_Return_Empty_List_If_No_Appointment_Found()
		{
			var query = new GetAppointmentsQuery
			{
				AppointmentId = Guid.NewGuid() // Not in DB
			};

			var handler = new GetAppointmentsHandler(_dbContext);

			var result = await handler.Handle(query, CancellationToken.None);

			Assert.Empty(result);
		}
	}

}
