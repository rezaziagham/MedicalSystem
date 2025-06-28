using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Infrastructure.Messaging
{
	public static class ServiceBusRegistration
	{
		public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddMassTransit(x =>
			{
				x.SetKebabCaseEndpointNameFormatter();
				x.AddConsumers(Assembly.GetExecutingAssembly());

				x.UsingRabbitMq((ctx, cfg) =>
				{
					cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
					{
						h.Username(configuration["RabbitMQ:Username"]);
						h.Password(configuration["RabbitMQ:Password"]);
					});

					cfg.ConfigureEndpoints(ctx);
				});
			});

			return services;
		}
	}

}
