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
	public class AppointmentDeletedConsumer : IConsumer<AppointmentDeletedConsumer>
	{
		private readonly ILogger<AppointmentDeletedConsumer> _logger;

		public AppointmentDeletedConsumer(ILogger<AppointmentDeletedConsumer> logger)
		{
			_logger = logger;
		}

		public Task Consume(ConsumeContext<AppointmentDeletedConsumer> context)
		{
			_logger.LogInformation($"Received event: AppointmentId={context.Message}");
			return Task.CompletedTask;
		}
	}

}
