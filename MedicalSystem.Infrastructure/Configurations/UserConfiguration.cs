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
	public abstract class UserConfiguration<T> : IEntityTypeConfiguration<T> where T : User
	{
		public virtual void Configure(EntityTypeBuilder<T> builder)
		{
			builder.Property(u => u.FirstName).IsRequired().HasMaxLength(64);
			builder.Property(u => u.LastName).IsRequired().HasMaxLength(64);
			builder.Property(u => u.Email).IsRequired().HasMaxLength(128);
			builder.Property(u => u.PasswordHash).IsRequired();
			builder.Property(u => u.Gender).IsRequired().HasMaxLength(16);
			builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(20);
			builder.Property(u => u.CreatedAt).IsRequired();
			builder.Property(u => u.UpdatedAt);
			builder.Property(u => u.DateOfBirth).IsRequired();
		}
	}
}
