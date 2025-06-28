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
	public class AppointmentCreatedConsumer : IConsumer<IAppointmentCreatedEvent>
	{
		private readonly ILogger<AppointmentCreatedConsumer> _logger;

		public AppointmentCreatedConsumer(ILogger<AppointmentCreatedConsumer> logger)
		{
			_logger = logger;
		}

		public Task Consume(ConsumeContext<IAppointmentCreatedEvent> context)
		{
			_logger.LogInformation($"Received event: AppointmentId={context.Message.AppointmentId}");
			return Task.CompletedTask;
		}
	}

}
