using AutoMapper;
using MedicalSystem.Application.Features.Appointments.Commands.Create;
using MedicalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Common
{
	public class Mappings : Profile
	{
		public Mappings() 
		{
			CreateMap<CreateAppointmentCommand, Appointment>();
		}

	}
}
