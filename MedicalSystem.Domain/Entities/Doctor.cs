using MedicalSystem.Domain.Common;
using MedicalSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Domain.Entities
{
	public class Doctor : User , IAggregateRoot
	{
		public Guid Id { get; set; }
		public DoctorSpecialty Specialty { get; set; }
		public string Bio { get; set; } = string.Empty;
		public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
		public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
	}
}
