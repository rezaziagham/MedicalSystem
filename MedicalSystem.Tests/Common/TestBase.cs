using AutoMapper;
using MassTransit;
using MedicalSystem.Application;
using MedicalSystem.Application.Common.Interfaces;
using MedicalSystem.Infrastructure.DependencyInjection;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;

namespace MedicalSystem.Tests.Common
{
	public abstract class TestBase : IDisposable
	{
		protected readonly IServiceProvider _serviceProvider;
		protected readonly ApplicationDbContext _dbContext;
		protected readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
		protected readonly IMapper _mapper;

		protected TestBase()
		{
			var services = new ServiceCollection();

			// ✅ Register In-Memory DB for Testing
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseInMemoryDatabase(Guid.NewGuid().ToString())); // unique DB for each test class

			// ✅ Bind IApplicationDbContext to actual DbContext
			services.AddScoped<IApplicationDbContext>(provider =>
				provider.GetRequiredService<ApplicationDbContext>());

			// ✅ Register Application & Infrastructure Services
			services.AddApplicationServices();
			services.AddInfrastructureServices(null); // Pass null or mock IConfiguration if needed

			_serviceProvider = services.BuildServiceProvider();

			// Resolve DB context for direct DB access in tests
			_dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();

			_mockPublishEndpoint = new Mock<IPublishEndpoint>();

			services.AddAutoMapper(typeof(MedicalSystem.Application.Common.Mappings)); // Replace with your AutoMapper profile class


		}

		// ✅ Dispose the DB after each test class to clean up
		public void Dispose()
		{
			_dbContext.Database.EnsureDeleted();
			_dbContext.Dispose();
		}
	}
}
