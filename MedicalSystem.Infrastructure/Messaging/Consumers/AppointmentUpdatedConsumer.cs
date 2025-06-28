using MassTransit;
using MedicalSystem.Application.Common.Messaging.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Infrastructure.Messaging.Consumers
{
	public class AppointmentUpdatedConsumer : IConsumer<IAppointmentUpdatedEvent>
	{
		private readonly ILogger<AppointmentUpdatedConsumer> _logger;

		public AppointmentUpdatedConsumer(ILogger<AppointmentUpdatedConsumer> logger)
		{
			_logger = logger;
		}

		public Task Consume(ConsumeContext<IAppointmentUpdatedEvent> context)
		{
			_logger.LogInformation($"Received event: AppointmentId={context.Message.AppointmentId}");
			return Task.CompletedTask;
		}
	}
}
