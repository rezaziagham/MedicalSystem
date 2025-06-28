using MedicalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Infrastructure.Configurations
{
	public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
	{
		public void Configure(EntityTypeBuilder<Appointment> builder)
		{
			builder.HasKey(a => a.AppointmentId);

			builder.Property(a => a.Status)
				.HasConversion<string>()
				.IsRequired();

			builder.Property(a => a.QueueNumber)
				.IsRequired();

			builder.Property(a => a.Notes)
				.HasMaxLength(1000);

			builder.HasOne(a => a.Doctor)
				.WithMany(d => d.Appointments)
				.HasForeignKey(a => a.DoctorId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(a => a.Patient)
				.WithMany(p => p.Appointments)
				.HasForeignKey(a => a.PatientId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(a => a.Schedule)
				.WithMany(s => s.Appointments)
				.HasForeignKey(a => a.ScheduleId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

}
