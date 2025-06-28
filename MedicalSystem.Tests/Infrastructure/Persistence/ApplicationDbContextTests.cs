using FluentAssertions;
using MassTransit;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;
using MedicalSystem.Infrastructure.Persistence;
using MedicalSystem.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class ApplicationDbContextTests : TestBase
{
	[Fact]
	public async Task Can_Add_Admin()
	{
		var admin = new Admin { FirstName = "Admin", LastName = "Test", Email = "admin@test.com", IsSuperAdmin = true };
		await _dbContext.Admins.AddAsync(admin);
		await _dbContext.SaveChangesAsync();

		var saved = await _dbContext.Admins.FirstOrDefaultAsync(a => a.Email == "admin@test.com");
		Assert.NotNull(saved);
		Assert.True(saved.IsSuperAdmin);
	}
	[Fact]
	public async Task Can_Add_Doctor_With_Schedule_And_Appointments()
	{
		var doctor = new Doctor { FirstName = "John", LastName = "Doe", Email = "doc@example.com", Specialty = DoctorSpecialty.Dermatology, Bio = "Experienced" };
		var patient = new Patient { FirstName = "Jane", LastName = "Doe", Email = "jane@example.com" };

		var schedule = new Schedule
		{
			ScheduleId = Guid.NewGuid(),
			Doctor = doctor,
			DayOfWeek = DayOfWeek.Monday,
			StartTime = TimeSpan.FromHours(9),
			EndTime = TimeSpan.FromHours(17),
			MaxAppointments = 10,
			IsReviewed = false,
			Status = ScheduleStatus.Available,
			CreatedAt = DateTime.UtcNow
		};

		var appointment = new Appointment
		{
			AppointmentId = Guid.NewGuid(),
			Doctor = doctor,
			Patient = patient,
			Schedule = schedule,
			QueueNumber = 1,
			Status = AppointmentStatus.Pending,
			Notes = "Initial consult",
			CreatedAt = DateTime.UtcNow
		};

		await _dbContext.Doctors.AddAsync(doctor);
		await _dbContext.Patients.AddAsync(patient);
		await _dbContext.Schedules.AddAsync(schedule);
		await _dbContext.Appointments.AddAsync(appointment);
		await _dbContext.SaveChangesAsync();

		var saved = await _dbContext.Appointments.Include(a => a.Doctor).Include(a => a.Patient).Include(a => a.Schedule).FirstOrDefaultAsync();
		Assert.NotNull(saved);
		Assert.Equal("Initial consult", saved.Notes);
		Assert.Equal("doc@example.com", saved.Doctor.Email);
	}
	[Fact]
	public async Task Can_Update_Schedule_Status()
	{
		var doctor = new Doctor { FirstName = "Update", LastName = "Doctor", Email = "update@doc.com", Specialty = DoctorSpecialty.Dermatology };
		var schedule = new Schedule
		{
			ScheduleId = Guid.NewGuid(),
			Doctor = doctor,
			DayOfWeek = DayOfWeek.Tuesday,
			StartTime = TimeSpan.FromHours(10),
			EndTime = TimeSpan.FromHours(16),
			MaxAppointments = 8,
			IsReviewed = false,
			Status = ScheduleStatus.Available,
			CreatedAt = DateTime.UtcNow
		};

		await _dbContext.Doctors.AddAsync(doctor);
		await _dbContext.Schedules.AddAsync(schedule);
		await _dbContext.SaveChangesAsync();

		schedule.Status = ScheduleStatus.Unavailable;
		schedule.IsReviewed = true;
		await _dbContext.SaveChangesAsync();

		var updated = await _dbContext.Schedules.FindAsync(schedule.ScheduleId);
		Assert.Equal(ScheduleStatus.Unavailable, updated.Status);
		Assert.True(updated.IsReviewed);
	}
	[Fact]
	public async Task Can_Delete_Patient_And_Cascade_Appointments()
	{
		var patient = new Patient { FirstName = "Cascade", LastName = "Test", Email = "cascade@test.com" };
		var doctor = new Doctor { FirstName = "CascadeDoc", LastName = "Test", Email = "cascadedoc@test.com", Specialty = DoctorSpecialty.Dermatology };
		var schedule = new Schedule
		{
			ScheduleId = Guid.NewGuid(),
			Doctor = doctor,
			DayOfWeek = DayOfWeek.Wednesday,
			StartTime = TimeSpan.FromHours(9),
			EndTime = TimeSpan.FromHours(12),
			MaxAppointments = 5,
			Status = ScheduleStatus.Available,
			CreatedAt = DateTime.UtcNow
		};

		var appointment = new Appointment
		{
			AppointmentId = Guid.NewGuid(),
			Doctor = doctor,
			Patient = patient,
			Schedule = schedule,
			QueueNumber = 2,
			Status = AppointmentStatus.Pending,
			CreatedAt = DateTime.UtcNow
		};

		await _dbContext.Doctors.AddAsync(doctor);
		await _dbContext.Patients.AddAsync(patient);
		await _dbContext.Schedules.AddAsync(schedule);
		await _dbContext.Appointments.AddAsync(appointment);
		await _dbContext.SaveChangesAsync();

		_dbContext.Patients.Remove(patient);
		await _dbContext.SaveChangesAsync();

		var checkAppointment = await _dbContext.Appointments.FindAsync(appointment.AppointmentId);
		Assert.Null(checkAppointment); // should be null if cascade delete works
	}
	[Fact]
	public async Task Should_Save_And_Load_All_Entities()
	{
		var admin = new Admin { FirstName = "System", LastName = "Admin", Email = "sysadmin@test.com" };
		var doctor = new Doctor { FirstName = "Multi", LastName = "Doctor", Email = "multi@doc.com", Specialty = DoctorSpecialty.Dermatology };
		var patient = new Patient { FirstName = "Multi", LastName = "Patient", Email = "multi@pat.com" };
		var schedule = new Schedule
		{
			ScheduleId = Guid.NewGuid(),
			Doctor = doctor,
			DayOfWeek = DayOfWeek.Friday,
			StartTime = TimeSpan.FromHours(8),
			EndTime = TimeSpan.FromHours(14),
			MaxAppointments = 6,
			Status = ScheduleStatus.Available,
			CreatedAt = DateTime.UtcNow
		};

		var appointment = new Appointment
		{
			AppointmentId = Guid.NewGuid(),
			Doctor = doctor,
			Patient = patient,
			Schedule = schedule,
			QueueNumber = 3,
			Status = AppointmentStatus.Pending,
			CreatedAt = DateTime.UtcNow
		};

		await _dbContext.Admins.AddAsync(admin);
		await _dbContext.Doctors.AddAsync(doctor);
		await _dbContext.Patients.AddAsync(patient);
		await _dbContext.Schedules.AddAsync(schedule);
		await _dbContext.Appointments.AddAsync(appointment);
		await _dbContext.SaveChangesAsync();

		Assert.Equal(1, await _dbContext.Admins.CountAsync());
		Assert.Equal(1, await _dbContext.Doctors.CountAsync());
		Assert.Equal(1, await _dbContext.Patients.CountAsync());
		Assert.Equal(1, await _dbContext.Schedules.CountAsync());
		Assert.Equal(1, await _dbContext.Appointments.CountAsync());
	}
	[Fact]
	public async Task Can_Insert_Patient_With_Appointments()
	{
		var doctor = new Doctor
		{
			FirstName = "Doc",
			LastName = "Who",
			Email = "doc@example.com",
			Specialty = DoctorSpecialty.Orthopedics,
			Bio = "Time traveler"
		};

		var patient = new Patient
		{
			FirstName = "John",
			LastName = "Doe",
			Email = "patient@example.com"
		};

		var schedule = new Schedule
		{
			Doctor = doctor,
			DayOfWeek = DayOfWeek.Tuesday,
			StartTime = new TimeSpan(10, 0, 0),
			EndTime = new TimeSpan(11, 0, 0),
			MaxAppointments = 5,
			Status = ScheduleStatus.Available,
			IsReviewed = true,
			CreatedAt = DateTime.UtcNow
		};

		var appointment = new Appointment
		{
			Doctor = doctor,
			Patient = patient,
			Schedule = schedule,
			QueueNumber = 1,
			Status = AppointmentStatus.Confirmed,
			CreatedAt = DateTime.UtcNow
		};

		_dbContext.Appointments.Add(appointment);
		await _dbContext.SaveChangesAsync();

		var result = await _dbContext.Appointments
			.Include(a => a.Doctor)
			.Include(a => a.Patient)
			.Include(a => a.Schedule)
			.FirstOrDefaultAsync();

		result.Should().NotBeNull();
		result!.Doctor.Email.Should().Be("doc@example.com");
		result.Patient.Email.Should().Be("patient@example.com");
	}
	[Fact]
	public async Task Appointment_Should_Have_Default_Status_Pending()
	{
		var appointment = new Appointment
		{
			Doctor = new Doctor
			{
				FirstName = "Test",
				LastName = "Doctor",
				Email = "testdoc@example.com",
				Specialty = DoctorSpecialty.Pediatrics,
				Bio = "Test Bio"
			},
			Patient = new Patient
			{
				FirstName = "Test",
				LastName = "Patient",
				Email = "testpatient@example.com"
			},
			Schedule = new Schedule
			{
				DayOfWeek = DayOfWeek.Saturday,
				StartTime = TimeSpan.FromHours(9),
				EndTime = TimeSpan.FromHours(12),
				MaxAppointments = 5,
				Status = ScheduleStatus.Available,
				IsReviewed = false,
				CreatedAt = DateTime.UtcNow
			},
			QueueNumber = 99,
			CreatedAt = DateTime.UtcNow
		};

		_dbContext.Appointments.Add(appointment);
		await _dbContext.SaveChangesAsync();

		var result = await _dbContext.Appointments.FirstAsync();
		result.Status.Should().Be(AppointmentStatus.Pending); // Default status
	}
	[Fact]
	public async Task Can_Update_Schedule_Status_And_IsReviewed()
	{
		var doctor = new Doctor
		{
			FirstName = "Update",
			LastName = "Doctor",
			Email = "updatedoc@example.com",
			Specialty = DoctorSpecialty.Cardiology,
			Bio = "Test"
		};

		var schedule = new Schedule
		{
			Doctor = doctor,
			DayOfWeek = DayOfWeek.Friday,
			StartTime = TimeSpan.FromHours(10),
			EndTime = TimeSpan.FromHours(14),
			MaxAppointments = 8,
			Status = ScheduleStatus.Available,
			IsReviewed = false,
			CreatedAt = DateTime.UtcNow
		};

		_dbContext.Schedules.Add(schedule);
		await _dbContext.SaveChangesAsync();

		// Update
		schedule.Status = ScheduleStatus.Unavailable;
		schedule.IsReviewed = true;
		schedule.UpdatedAt = DateTime.UtcNow;

		await _dbContext.SaveChangesAsync();

		var result = await _dbContext.Schedules.FirstAsync();
		result.Status.Should().Be(ScheduleStatus.Unavailable);
		result.IsReviewed.Should().BeTrue();
		result.UpdatedAt.Should().NotBeNull();
	}
	[Fact]
	public async Task Can_Delete_Patient()
	{
		var patient = new Patient
		{
			FirstName = "Delete",
			LastName = "Patient",
			Email = "deletepatient@example.com"
		};

		_dbContext.Patients.Add(patient);
		await _dbContext.SaveChangesAsync();

		_dbContext.Patients.Remove(patient);
		await _dbContext.SaveChangesAsync();

		var result = await _dbContext.Patients.FirstOrDefaultAsync(p => p.Email == "deletepatient@example.com");
		result.Should().BeNull();
	}
}
