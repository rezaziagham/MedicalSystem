using FluentValidation;
using MedicalSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Features.Appointments.Commands.Delete
{
	internal class DeleteAppointmentCommandValidator : AbstractValidator<DeleteAppointmentCommand>
	{
		public DeleteAppointmentCommandValidator()
		{
			RuleFor(a => a.Id).NotEmpty();
		}
	}
}
