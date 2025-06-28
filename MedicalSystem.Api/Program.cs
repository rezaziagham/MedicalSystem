using MedicalSystem.Api.Configurations;
using MedicalSystem.Api.Endpoints;
using MedicalSystem.Api.Extentions;
using MedicalSystem.Api.Services;
using MedicalSystem.Application;
using MedicalSystem.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
// =====================================
 // 1. Configure Services
 // =====================================
ConfigureServices(builder);

var app = builder.Build();

// =====================================
// 2. Configure Middleware Pipeline
// =====================================
ConfigureMiddleware(app);

// =====================================
// 3. Map Endpoints
// =====================================
app.MapAppointmentEndpoints(); // Example endpoint group

app.Run();

// =====================================
// Helper Methods (Keep Main Flow Clean)
// =====================================

void ConfigureServices(WebApplicationBuilder builder)
{
	// Swagger/OpenAPI
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	// Application & Infrastructure Layers
	builder.Services.AddApplicationServices();
	builder.Services.AddInfrastructureServices(builder.Configuration);
	builder.Services.MigrateDatabase();

	// JWT Authentication
	var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
	builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
	builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

	builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = jwtSettings.Issuer,
				ValidAudience = jwtSettings.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
			};
		});

	// Authorization Policies
	builder.Services.AddAuthorization(options =>
	{
		options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
		options.AddPolicy("DoctorPolicy", policy => policy.RequireRole("Doctor"));
		options.AddPolicy("PatientPolicy", policy => policy.RequireRole("Patient"));
	});
}

void ConfigureMiddleware(WebApplication app)
{
	// Development Tools
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	// Security & HTTPS
	app.UseHttpsRedirection();
	app.UseAuthentication();
	app.UseAuthorization();
}