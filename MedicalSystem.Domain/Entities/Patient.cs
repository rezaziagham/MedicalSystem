using MedicalSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Domain.Entities
{
	public class Patient : User , IAggregateRoot
	{
		public Guid Id { get; set; }
		public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
	}
}
