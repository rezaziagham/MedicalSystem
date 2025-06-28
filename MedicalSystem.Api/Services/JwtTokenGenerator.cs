using MedicalSystem.Api.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicalSystem.Api.Services
{
	public interface IJwtTokenGenerator
	{
		string GenerateToken(string userId, string email, string role);
	}

	public class JwtTokenGenerator : IJwtTokenGenerator
	{
		private readonly JwtSettings _jwtSettings;
		public JwtTokenGenerator(IOptions<JwtSettings> options)
		{
			_jwtSettings = options.Value;
		}

		public string GenerateToken(string userId, string email, string role)
		{
			var claims = new[]
			{
			new Claim(JwtRegisteredClaimNames.Sub, userId),
			new Claim(JwtRegisteredClaimNames.Email, email),
			new Claim(ClaimTypes.Role, role),
		};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

			var token = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: expires,
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}

}
