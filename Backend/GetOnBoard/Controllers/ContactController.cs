using GetOnBoard.DAL;
using GetOnBoard.DAL.Extensions;
using GetOnBoard.DAL.Repositories;
using GetOnBoard.Models;
using GetOnBoard.Models.Models;
using GetOnBoard.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetOnBoard.Controllers
{
	
	public class ContactController : Controller
	{
		private readonly GetOnBoardDbContext _db;
		private readonly ILogger<GameSessionsController> _logger;
		
		public ContactController(GetOnBoardDbContext db, ILogger<GameSessionsController> logger)
		{
			_db = db;
			_logger = logger;
		}

		[HttpPost]
		[Route("Contact")]
		public async Task<IActionResult> AddNewMessageFromUsers([FromBody] AddMessageContactViewModel model)
		{
			Contact contact = new Contact();
			contact.Email = model.Email;
			contact.Message = model.Message;

			await _db.AddMessageFromUser(contact);
			return Ok();
		}

	}
}
