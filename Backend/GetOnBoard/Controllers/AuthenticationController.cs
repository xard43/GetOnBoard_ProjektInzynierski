using GetOnBoard.DAL;
using GetOnBoard.Tokens;
using GetOnBoard.Logging;
using GetOnBoard.Models;
using GetOnBoard.Models.ViewModels;
using GetOnBoard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GetOnBoard.Controllers
{
	public class AuthenticationController : Controller
	{

		private readonly GetOnBoardDbContext _dbContext;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _configuration;
		private readonly ILogger _logger;


		public AuthenticationController(GetOnBoardDbContext dbContext,
										UserManager<ApplicationUser> userManager,
										IConfiguration configuration,
										ILogger<AuthenticationController> logger)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_configuration = configuration;
			_logger = logger;
		}

		/// <summary>
		/// Register user in the application
		/// </summary>
		/// <param name="userFromBody"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[Route("register")]
		[HttpPost]
		public async Task<ActionResult> Register([FromBody] RegisterViewModel userFromBody)
		{
			var user = new ApplicationUser
			{
				UserName = userFromBody.Username,
				Email = userFromBody.Email,
				SecurityStamp = Guid.NewGuid().ToString()
			};

			var result = await _userManager.CreateAsync(user, userFromBody.Password);
			if (result.Succeeded)
			{
				_logger.LogInformation(LoggingEvents.CreateUser, "Create user {ID}", user.UserName);

				UserProfile profile = new UserProfile
				{
					ApplicationUser = user,
					Avatar = "https://getonboard.blob.core.windows.net/profile-pictures/default-user.png"

				};
				_dbContext.UserProfiles.Add(profile);

			}
			else
			{
				string errorMessage = String.Empty;
				foreach (IdentityError error in result.Errors)
				{
					errorMessage += error.Description;
				}
				throw new RequestException(HttpStatusCode.BadRequest, errorMessage, LoggingEvents.CreateUser);
			}
			_dbContext.SaveChanges();

			

			return Ok(await Login(new LoginViewModel { Username = userFromBody.Username, Password = userFromBody.Password }));
		}

		/// <summary>
		/// Login User in the application
		/// </summary>
		/// <param name="userFromView"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[Route("login")]
		[HttpPost]
		public async Task<ActionResult> Login([FromBody] LoginViewModel userFromView)
		{
			var user = await _userManager.FindByNameAsync(userFromView.Username);
			if (user != null && await _userManager.CheckPasswordAsync(user, userFromView.Password))
			{
				JwtSecurityToken accessToken = await TokenFactory.GenerateJwtToken(user, _configuration, _userManager);

				//generate refresh token
				var refreshToken = TokenFactory.GenerateRefreshToken();
				var refreshTokenFromDb = _dbContext.UserTokens.Find(user.Id, "GetOnBoard", "refreshToken");

				if (refreshTokenFromDb == null)
				{
					_dbContext.UserTokens.Add(new IdentityUserToken<string> { UserId = user.Id, Name = "refreshToken", Value = refreshToken, LoginProvider = "GetOnBoard" });
				}
				else
				{
					refreshTokenFromDb.Value = refreshToken;
				}
				await _dbContext.SaveChangesAsync();

				_logger.LogInformation(LoggingEvents.LoginUser, "User: {ID} successfully login. Token: {token}", user.UserName, new JwtSecurityTokenHandler().WriteToken(accessToken));

				return Ok(new JWTUserTokenViewModel(new JwtSecurityTokenHandler().WriteToken(accessToken), accessToken.ValidTo, refreshToken));
			}
			else
			{
				_logger.LogInformation(LoggingEvents.LoginUser, "Login failed for UserName: {ID} and Password: {password}", userFromView.Username, userFromView.Password);

			}
			return Unauthorized();
		}

		[AllowAnonymous]
		[HttpPost("refreshToken")]
		public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
		{
			if (!ModelState.IsValid) { return BadRequest(ModelState); }


			SecurityToken securityToken;
			ClaimsPrincipal cp = new JwtSecurityTokenHandler().ValidateToken(request.AccessToken, new TokenValidationParameters()
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidAudience = _configuration["Jwt:Audience"],
				ValidIssuer = _configuration["Jwt:Issuer"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"])),
				ValidateLifetime = false,
			}, out securityToken);


			if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException("Invalid token");

			if (cp != null)
			{
				//find user
				var id = cp.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
				var user = await _userManager.FindByIdAsync(id.Value);

				var oldRefreshToken = _dbContext.UserTokens.Find(user.Id, "GetOnBoard", "refreshToken");

				if (oldRefreshToken != null)
				{
					JwtSecurityToken accessToken = await TokenFactory.GenerateJwtToken(user, _configuration, _userManager);
					var refreshToken = TokenFactory.GenerateRefreshToken();

					_dbContext.UserTokens.Remove(oldRefreshToken);
					_dbContext.UserTokens.Add(new IdentityUserToken<string> { UserId = user.Id, Name = "refreshToken", Value = refreshToken, LoginProvider = "GetOnBoard" });

					await _dbContext.SaveChangesAsync();

					return Ok(new JWTUserTokenViewModel(new JwtSecurityTokenHandler().WriteToken(accessToken), accessToken.ValidTo, refreshToken));
				}
			}
			return Unauthorized();
		}
	}

}
