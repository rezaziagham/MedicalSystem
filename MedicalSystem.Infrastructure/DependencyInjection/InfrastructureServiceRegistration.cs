using MedicalSystem.Application.Common.Interfaces;
using MedicalSystem.Infrastructure.Messaging;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Infrastructure.DependencyInjection
{
	public static class InfrastructureServiceRegistration
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			var dbProvider = configuration["DatabaseProvider"];
			Console.WriteLine(dbProvider);
			if (dbProvider == "SqlServer")
			{
				var connStr = configuration.GetConnectionString("DefaultConnection");
				if (string.IsNullOrWhiteSpace(connStr))
					throw new Exception("Connection string is null or empty!");

				Console.WriteLine($"Using connection string: {connStr}");

				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(connStr));
			}
			else if (dbProvider == "PostgreSQL")
			{
				var connStr = configuration.GetConnectionString("PostgreConnection");
				if (string.IsNullOrWhiteSpace(connStr))
					throw new Exception("PostgreConnection string is null or empty!");

				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseNpgsql(connStr));
			}

			services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
			services.AddMessaging(configuration);
			return services;
		}
	}

}
