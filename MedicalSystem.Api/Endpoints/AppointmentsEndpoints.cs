using MediatR;
using MedicalSystem.Application.Features.Appointments.Commands.Create;

namespace MedicalSystem.Api.Endpoints
{
	public static class AppointmentsEndpoints
	{
		public static void MapAppointmentEndpoints(this IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("/api/appointments").WithTags("Appointments");

			group.MapPost("/", async (
				CreateAppointmentCommand command,
				ISender sender) =>
			{
				var id = await sender.Send(command);
				return Results.Ok(id);
			}).RequireAuthorization();
		}
	}

}
