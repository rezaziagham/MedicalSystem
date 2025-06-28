using MassTransit;
using MedicalSystem.Application.Common.Messaging.Events;
using MedicalSystem.Application.Features.Appointments.Commands.Delete;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;
using MedicalSystem.Shared;
using MedicalSystem.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
public class DeleteAppointmentCommandTests : TestBase
{
	[Fact]
	public async Task Handle_ShouldDeleteAppointment_WhenExists()
	{
		// Arrange
		var appointment = new Appointment
		{
			AppointmentId = Guid.NewGuid(),
			DoctorId = Guid.NewGuid(),
			PatientId = Guid.NewGuid(),
			ScheduleId = Guid.NewGuid(),
			Status = AppointmentStatus.Pending,
			Notes = "Initial"
		};

		_dbContext.Appointments.Add(appointment);
		await _dbContext.SaveChangesAsync();

		var command = new DeleteAppointmentCommand(appointment.AppointmentId);

		var handler = new DeleteAppointmentHandler(_dbContext, _mockPublishEndpoint.Object , _mapper);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		var deleted = await _dbContext.Appointments.FindAsync(appointment.AppointmentId);
		Assert.Null(deleted); // should be deleted
		Assert.Equal(appointment.AppointmentId, result);

		_mockPublishEndpoint.Verify(p => p.Publish<IAppointmentDeletedEvent>(
			It.Is<IAppointmentDeletedEvent>(e => e.AppointmentId == appointment.AppointmentId),
			It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task Handle_ShouldThrowNotFoundException_WhenAppointmentDoesNotExist()
	{
		// Arrange
		var nonExistentId = Guid.NewGuid();
		var command = new DeleteAppointmentCommand(nonExistentId);

		var handler = new DeleteAppointmentHandler(_dbContext, _mockPublishEndpoint.Object, _mapper);

		// Act & Assert
		await Assert.ThrowsAsync<NotFoundException>(() =>
			handler.Handle(command, CancellationToken.None));
	}
}
