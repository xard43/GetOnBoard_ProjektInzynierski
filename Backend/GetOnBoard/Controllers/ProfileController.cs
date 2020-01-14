using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetOnBoard.DAL;
using GetOnBoard.DAL.Extensions;
using GetOnBoard.DAL.Repositories;
using GetOnBoard.Models;
using GetOnBoard.Models.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetOnBoard.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	public class ProfileController : Controller
	{
		private readonly GetOnBoardDbContext _db;
		private readonly ILogger<ProfileController> _logger;
		private readonly IProfileRepository _profileRepository;

		public ProfileController(GetOnBoardDbContext db, ILogger<ProfileController> logger, IProfileRepository repository)
		{
			_db = db;
			_logger = logger;
			_profileRepository = repository;
		}

		[HttpGet]
		[Route("{userID}")]
		public ActionResult GetProfile(string userID)
		{
			return Ok(_profileRepository.GetUserProfile(userID, User));
		}

		[HttpPut]
		public async Task <IActionResult> UpdateUserProfile([FromBody] UpdateProfileViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			await _profileRepository.UpdateDataInProfile(model);
			return Ok();
		}

		
	}
}