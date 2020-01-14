using GetOnBoard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GetOnBoard.Tokens
{
	internal static class TokenFactory
	{
		public static string GenerateRefreshToken(int size = 32)
		{
			var randomNumber = new byte[size];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}

		public static async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user, IConfiguration configuration, UserManager<ApplicationUser> userManager)
		{
			var signinKey = new SymmetricSecurityKey(
			  Encoding.UTF8.GetBytes(configuration["Jwt:SigningKey"]));

			int expiryInMinutes = Convert.ToInt32(configuration["Jwt:ExpiryInMinutes"]);

			var claims = await GetValidClaims(user, userManager);

			var accessToken = new JwtSecurityToken(
				issuer: configuration["Jwt:Issuer"],
				audience: configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
				signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
				);
			return accessToken;
		}

		private static async Task<List<Claim>> GetValidClaims(ApplicationUser user, UserManager<ApplicationUser> userManager)
		{
			IdentityOptions _options = new IdentityOptions();
			var claims = new List<Claim>
				{
					new Claim(_options.ClaimsIdentity.UserNameClaimType, user.UserName),
					new Claim(_options.ClaimsIdentity.UserIdClaimType, user.Id),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				};

			var userClaims = await userManager.GetClaimsAsync(user);

			claims.AddRange(userClaims);
			return claims;
		}
	}
}
