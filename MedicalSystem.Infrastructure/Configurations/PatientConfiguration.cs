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
	public class PatientConfiguration : IEntityTypeConfiguration<Patient>
	{
		public void Configure(EntityTypeBuilder<Patient> builder)
		{
			builder.HasKey(p => p.Id);

			builder.HasMany(p => p.Appointments)
				.WithOne(a => a.Patient)
				.HasForeignKey(a => a.PatientId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}

}
