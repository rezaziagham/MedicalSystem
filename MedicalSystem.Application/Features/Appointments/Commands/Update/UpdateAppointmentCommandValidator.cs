using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Features.Appointments.Commands.Update
{
	internal class UpdateAppointmentCommandValidator : AbstractValidator<UpdateAppointmentCommand>
	{
		public UpdateAppointmentCommandValidator()
		{
			RuleFor(a => a.Appointment).NotEmpty();
			RuleFor(a => a.Id).NotEmpty();
		}
	}
}
