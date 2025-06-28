﻿using MedicalSystem.Domain.Common;
using MedicalSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Domain.Entities
{
	public abstract class User : IAuditableEntity
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string FullName => $"{FirstName} {LastName}";
		public string Email { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public UserRole Role { get; set; }
		public string Gender { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime DateOfBirth { get; set; }
	}
}
