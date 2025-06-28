using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Features.Appointments.Commands.Create
{
	public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
	{
		public CreateAppointmentCommandValidator() 
		{
			RuleFor(a=>a.ScheduleId).NotEmpty();
			RuleFor(a=>a.PatientId).NotEmpty();
		}
	}
}
