using MassTransit;
using MedicalSystem.Application.Common.Interfaces;
using MedicalSystem.Application.Common.Messaging.Events;
using MedicalSystem.Application.Features.Appointments.Commands;
using MedicalSystem.Application.Features.Appointments.Commands.Create;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Tests.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Tests.Application.Appointments
{
	public class CreateAppointmentCommandTests : TestBase
	{
		[Fact]
		public async Task Should_Create_Appointment_Successfully()
		{
			// Arrange
			var schedule = new Schedule
			{
				ScheduleId = Guid.NewGuid(),
				DoctorId = Guid.NewGuid(),
				DayOfWeek = DayOfWeek.Monday,
				StartTime = TimeOnly.Parse("08:00").ToTimeSpan(),
				EndTime = TimeOnly.Parse("10:00").ToTimeSpan()
			};
			await _dbContext.Schedules.AddAsync(schedule);
			await _dbContext.SaveChangesAsync();

			var command = new CreateAppointmentCommand
			{
				ScheduleId = schedule.ScheduleId,
				PatientId = Guid.NewGuid(),
				Notes = "Test Note"
			};

			var handler = new CreateAppointmentHandler(_dbContext, _mockPublishEndpoint.Object ,_mapper);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			var appointment = await _dbContext.Appointments.FindAsync(result);
			Assert.NotNull(appointment);
			Assert.Equal(command.Notes, appointment.Notes);
			Assert.Equal(schedule.DoctorId, appointment.DoctorId);

			_mockPublishEndpoint.Verify(p =>
				p.Publish<IAppointmentCreatedEvent>(
					It.IsAny<object>(), It.IsAny<CancellationToken>()),
				Times.Once);
		}

		[Fact]
		public async Task Should_Throw_When_Schedule_Not_Found()
		{
			var command = new CreateAppointmentCommand
			{
				ScheduleId = Guid.NewGuid(), // Not in DB
				PatientId = Guid.NewGuid(),
				Notes = "Should fail"
			};

			var handler = new CreateAppointmentHandler(_dbContext, _mockPublishEndpoint.Object,_mapper);

			await Assert.ThrowsAsync<NullReferenceException>(() =>
				handler.Handle(command, CancellationToken.None));
		}

		[Fact]
		public async Task Should_Throw_When_PatientId_Is_Empty()
		{
			var schedule = new Schedule
			{
				ScheduleId = Guid.NewGuid(),
				DoctorId = Guid.NewGuid(),
				DayOfWeek = DayOfWeek.Tuesday,
				StartTime = TimeOnly.Parse("10:00").ToTimeSpan(),
				EndTime = TimeOnly.Parse("12:00").ToTimeSpan()
			};
			await _dbContext.Schedules.AddAsync(schedule);
			await _dbContext.SaveChangesAsync();

			var command = new CreateAppointmentCommand
			{
				ScheduleId = schedule.ScheduleId,
				PatientId = Guid.Empty, // Invalid
				Notes = "Invalid patient"
			};

			var handler = new CreateAppointmentHandler(_dbContext, _mockPublishEndpoint.Object,_mapper);

			// Optional: Add validation logic to handler for cleaner exception
			await Assert.ThrowsAsync<ArgumentException>(() =>
				handler.Handle(command, CancellationToken.None));
		}
	}


}
