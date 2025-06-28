using MedicalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Common.Interfaces
{
	public interface IApplicationDbContext
	{
		DbSet<Appointment> Appointments { get; }
		DbSet<Doctor> Doctors { get; }
		DbSet<Patient> Patients { get; }
		DbSet<Schedule> Schedules { get; }
		DbSet<Admin> Admins { get; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}

}
