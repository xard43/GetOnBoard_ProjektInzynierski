using GetOnBoard.DAL.Repositories;
using GetOnBoard.DAL.Extensions;
using GetOnBoard.Logging;
using GetOnBoard.Models;
using GetOnBoard.ViewModels.GameSession;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using GetOnBoard.DAL;
using System.Collections.Generic;
using GetOnBoard.Models.ViewModels.GameSession;

namespace GetOnBoard.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	public class GameSessionsController : Controller
	{
		private readonly GetOnBoardDbContext _db;
		private readonly ILogger<GameSessionsController> _logger;
		private readonly IGameSessionsRepository _gameSessionRepository;

		public GameSessionsController(GetOnBoardDbContext db, ILogger<GameSessionsController> logger, IGameSessionsRepository repository)
		{
			_db = db;
			_logger = logger;
			_gameSessionRepository = repository;
		}

		#region GameSessions CRUD
		/// <summary>
		/// Returns List of all GameSessions in Json
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GameSessions()
		{
			var eventsList = await _gameSessionRepository.GetGameSessionListAsync();
			return new JsonResult(eventsList);
		}

		/// <summary>
		/// Get GameSession by ID
		/// </summary>
		/// <param name="id">GameSession ID</param>
		/// <returns></returns>
		[HttpGet("{id}")]
		[SwaggerResponse((int)HttpStatusCode.NotFound, "Returns when game session was not found")]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, "Returns when something went wrong with getting game session from server")]

		public async Task<IActionResult> GetGameSession([FromRoute] int id)
		{
			var singleGameSession = await _gameSessionRepository.GetSingleGameSessionAsync(id, User);
			return Ok(singleGameSession);
		}

		/// <summary>
		/// Adding new GameSession
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> PostGameSession([FromBody] CreateNewGSViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var id = await _gameSessionRepository.CreateNewGameSession(model, User);

			return Ok(new {Id = id });
		}

		[HttpGet]
		[Route("GetDictionaryBoardGames")]
		public ActionResult GetDictionaryBoardGames()
		{
			return new JsonResult(_gameSessionRepository.GetBoardGamesDictionary());

		}
		
		/// <summary>
		/// Deletes specified GameSession
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteGameSession([FromRoute] int id)
		{

			await _gameSessionRepository.DeleteGameSessionByAdmin(id, User);

			return Ok();
		}
		#endregion

		/// <summary>
		/// Joined to exist GameSession
		/// </summary>
		/// <param name="gameSessionID"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("JoinGameSession/{gameSessionID}")]
		public async Task<IActionResult> JoinGameSession(int gameSessionID)
		{

			var singleGameSessionAfterJoin = await _gameSessionRepository.JoinToCurrentGameSession(gameSessionID, User);

			return Ok(singleGameSessionAfterJoin);
		}


		[HttpPost]
		[Route("LeaveGameSession/{gameSessionID}")]
		public async Task<IActionResult> LeaveGameSession(int gameSessionID)
		{
			var singleGameSessionAfterLeft = await _gameSessionRepository.LeaveCurrentGameSession(gameSessionID, User);

			return Ok(singleGameSessionAfterLeft);

		}

		[HttpPost]
		[Route("SearchGameSessions")]
		public async Task<IActionResult> SearchGameSessionsButton([FromBody] SearchGameSessionsViewModel model)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var eventsList =  await _gameSessionRepository.SearchGameSessions(model);
			
			return new JsonResult(eventsList);
			
		}

		

		[HttpPost]
		[Route("DeletePlayerFromGameSessionByAdmin")]
		public async Task<IActionResult> DeletePlayerFromGameSessionByAdmin([FromBody] DeletePlayerFromGS model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var singleGameSessionAfterLeft = await _gameSessionRepository.DeletePlayerFromCurrentGameByAdmin(model.GameSessionID, model.UserID, User);

			return Ok(singleGameSessionAfterLeft);

		}

		[HttpGet]
		[Route("GetMyGamesSessions")]
		public async Task<IActionResult> GetMyGamesSessions()
		{
			var singleGameSessionAfterLeft = await _gameSessionRepository.GetMyGameSessionListAsync(User);

			return Ok(singleGameSessionAfterLeft);

		}
	}
}