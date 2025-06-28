using MedicalSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Domain.Entities
{
	public class Admin : User , IAggregateRoot
	{
		public Guid Id { get; set; }
		public bool IsSuperAdmin { get; set; } = false;
		public string? Notes { get; set; }
		public DateTime? LastLoginAt { get; set; }
	}
}
