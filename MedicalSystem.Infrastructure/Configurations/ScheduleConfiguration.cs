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
	public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
	{
		public void Configure(EntityTypeBuilder<Schedule> builder)
		{
			builder.HasKey(s => s.ScheduleId);

			builder.Property(s => s.DayOfWeek)
				.IsRequired();

			builder.Property(s => s.StartTime)
				.IsRequired();

			builder.Property(s => s.EndTime)
				.IsRequired();

			builder.Property(s => s.MaxAppointments)
				.IsRequired();

			builder.Property(s => s.IsReviewed)
				.IsRequired();

			builder.Property(d => d.Status)
				.HasConversion<string>()
				.IsRequired();

			builder.HasMany(s => s.Appointments)
				.WithOne(a => a.Schedule)
				.HasForeignKey(a => a.ScheduleId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
