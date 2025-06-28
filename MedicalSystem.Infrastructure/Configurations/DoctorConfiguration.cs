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
	public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
	{
		public void Configure(EntityTypeBuilder<Doctor> builder)
		{
			builder.HasKey(d => d.Id);

			builder.Property(d => d.Specialty)
				.HasConversion<string>()
				.IsRequired();

			builder.Property(d => d.Bio)
				.HasMaxLength(1000);

			builder.HasMany(d => d.Schedules)
				.WithOne(s => s.Doctor)
				.HasForeignKey(s => s.DoctorId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(d => d.Appointments)
				.WithOne(a => a.Doctor)
				.HasForeignKey(a => a.DoctorId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}

}
