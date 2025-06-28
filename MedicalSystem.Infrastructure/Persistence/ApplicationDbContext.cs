using MedicalSystem.Application.Common.Interfaces;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Infrastructure.Persistence
{
	public class ApplicationDbContext : DbContext, IApplicationDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Appointment> Appointments => Set<Appointment>();
		public DbSet<Doctor> Doctors => Set<Doctor>();
		public DbSet<Patient> Patients => Set<Patient>();
		public DbSet<Schedule> Schedules => Set<Schedule>();
		public DbSet<Admin> Admins => Set<Admin>();

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await base.SaveChangesAsync(cancellationToken);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new DoctorConfiguration());
			modelBuilder.ApplyConfiguration(new PatientConfiguration());
			modelBuilder.ApplyConfiguration(new AdminConfiguration());
			modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
			modelBuilder.ApplyConfiguration(new ScheduleConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}

}
