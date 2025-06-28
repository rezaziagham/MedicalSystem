﻿using MedicalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Infrastructure.Configurations
{
	public class AdminConfiguration : IEntityTypeConfiguration<Admin>
	{
		public void Configure(EntityTypeBuilder<Admin> builder)
		{
			builder.HasKey(a => a.Id);

			builder.Property(a => a.IsSuperAdmin)
				.IsRequired();

			builder.Property(a => a.Notes)
				.HasMaxLength(1000);

			builder.Property(a => a.LastLoginAt)
				.IsRequired(false);
		}
	}


}
