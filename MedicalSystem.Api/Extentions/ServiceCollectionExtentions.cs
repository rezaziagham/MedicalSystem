using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalSystem.Api.Extentions;

public static class ServiceCollectionExtentions
{
	public static IServiceCollection MigrateDatabase(this IServiceCollection services)
	{
		var serviceProvider = services.BuildServiceProvider().CreateScope();
		using (var scope = serviceProvider.ServiceProvider.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			dbContext.Database.Migrate();
		}
		return services;
	}
}

