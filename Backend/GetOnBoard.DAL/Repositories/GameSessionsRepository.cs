using GetOnBoard.DAL.Extensions;
using GetOnBoard.DAL.Helper;
using GetOnBoard.Logging;
using GetOnBoard.Models;
using GetOnBoard.Models.ViewModels.GameSession;
using GetOnBoard.ViewModels;
using GetOnBoard.ViewModels.GameSession;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Repositories
{
	public class GameSessionsRepository : IGameSessionsRepository
	{
		private readonly ILogger<GameSessionsRepository> _logger;
		private readonly GetOnBoardDbContext _db;
		private readonly IBlobStorageRepository _blobStorage;

		public GameSessionsRepository(ILogger<GameSessionsRepository> logger, GetOnBoardDbContext db, IBlobStorageRepository blobStorage)
		{
			_logger = logger;
			_db = db;
			_blobStorage = blobStorage;
		}
		#region GetGameSessionList
		public async Task<List<GameSessionViewModel>> GetGameSessionListAsync()
		{
			var gameSessionList = await _db.GameSessions.GetAllGameSessionsAsync();

			var gameSessionViewModels = GameSessionMethods.GetListGameSessionsViewModel(gameSessionList);

			_logger.LogInformation(LoggingEvents.GetGameSessionList, "Returned {Count} GameSessions", gameSessionViewModels.Count());
			return gameSessionViewModels;
		}
		#endregion

		#region GetSingleGameSession
		public async Task<SingleGameSessionViewModel> GetSingleGameSessionAsync(int gameSessionID, ClaimsPrincipal user)
		{
			GameSession gameSession = await _db.GameSessions.GetActiveGameSessionByIdAsync(gameSessionID);

			#region UserID
			Claim userIdClaim = user.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault();
			string userID;
			if (userIdClaim == null)
			{
				var message = "User does not have valid ID claim";
				throw new RequestException(HttpStatusCode.BadRequest, message, LoggingEvents.GetActiveGameSessionByIdAsync);
			}
			else
			{
				userID = userIdClaim.Value;
			}
			#endregion
			#region Players
			List<PlayerSingleGSViewModel> playersGSList = new List<PlayerSingleGSViewModel>();
			List<ApplicationUser> gameSessionPlayers = _db.GameSessionApplicationUsers.GetGameSessionPlayers(gameSession);

			bool isUserInGame = false;
			foreach (var player in gameSessionPlayers)
			{
				if (player.Id == userID)
				{
					isUserInGame = true;
				}
				var playerControllerModel = new PlayerSingleGSViewModel
				{
					ID = player.Id,
					UserName = player.UserName,
					FirstName = player.Profile.FirstName,
					LastName = player.Profile.LastName,
					AvatarPlayer = player.Profile.Avatar
				};
				playersGSList.Add(playerControllerModel);
			}
			#endregion
			#region Admin
			var adminEvent = _db.Users.GetEventAdmin(gameSession);

			var avatarAdmin = adminEvent.Profile.Avatar;

			//var playerVMAdmin = new PlayerSingleGSViewModel
			//{
			//	ID = adminEvent.Id,
			//	UserName = adminEvent.UserName,
			//	FirstName = adminEvent.Profile.FirstName,
			//	LastName = adminEvent.Profile.LastName,
			//	AvatarPlayer = avatarAdmin
			//};

			//playersGSList.Add(playerVMAdmin);

			bool isAdminEvent = false;
			if (userID == gameSession.UserAdminID)
			{
				isAdminEvent = true;
				isUserInGame = true;
			}
			#endregion
			#region BoardGames
			List<BoardGameSingleGSViewModel> boardgamesGSList = new List<BoardGameSingleGSViewModel>();
			var gameSessionBoardGames = _db.GameSessionBoardGames.GetBoardGamesFromGameSession(gameSession);
			//TODO: Zrobić projekcje? 
			//.Select(x => new BoardGameSingleGSViewModel()
			// { ID = x.ID });

			foreach (var boardGame in gameSessionBoardGames)
			{
				var boardGameVM = new BoardGameSingleGSViewModel
				{
					ID = boardGame.ID,
					Age = boardGame.Age,
					GameTimeMax = boardGame.GameTimeMax,
					GameTimeMin = boardGame.GameTimeMin,
					Name = boardGame.Name,
					Category = boardGame.Categories,
					PlayersMin = boardGame.PlayersMin,
					PlayersMax = boardGame.PlayersMax,
					Description = boardGame.Description,
					BoardGameAvatar = boardGame.ImageBoardGame,
				};

				boardgamesGSList.Add(boardGameVM);
			}
			#endregion

			_logger.LogInformation(LoggingEvents.GetActiveGameSessionByIdAsync, "Returning a GameSession details completed. GameSession ID: {ID}", gameSessionID);
			return new SingleGameSessionViewModel()
			{
				ID = gameSession.ID,
				City = gameSession.City,
				Address = gameSession.Address,
				Name = gameSession.Name,
				Description = gameSession.Description,
				TimeStart = gameSession.TimeStart,
				TimeEnd = gameSession.TimeEnd,
				Slots = gameSession.Slots,
				SlotsFree = gameSession.SlotsFree,
				AdminID = adminEvent.Id,
				IsCurrentUserInGame = isUserInGame,
				IsAdmin = isAdminEvent,
				AdminAvatar = avatarAdmin,
				Players = playersGSList,
				BoardGamesEvent = boardgamesGSList
			};

		}
		#endregion

		#region CreteNewGameSession
		public async Task<int> CreateNewGameSession(CreateNewGSViewModel model, ClaimsPrincipal User)
		{
			string userID = User.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault().Value;

			var userAdmin = _db.Users.GetUserById(userID);
			int boardGameID = model.BoardGameID;
			if (model.IsCustomGame)
			{
				string boardGameImage = "";
				Guid boardGameGuid = Guid.NewGuid();
				// add boardgame image to blobStorage
				if (!string.IsNullOrEmpty(model.CustomGameImage))
				{
					byte[] imageBoardGameBytes = Convert.FromBase64String(model.CustomGameImage);
					Stream stream = new MemoryStream(imageBoardGameBytes);
					await _blobStorage.Save(stream, $"{boardGameGuid}.jpg");
					boardGameImage = _blobStorage.Load($"{boardGameGuid}.jpg").Uri.AbsoluteUri;
					
				}
				else
				{
					boardGameImage = "https://zenit.org/wp-content/uploads/2018/05/no-image-icon.png";
				}

				//Create new board game
				var newBoardGame = new BoardGame
				{
					Name = model.CustomGameName,
					ImageBoardGame = boardGameImage,
					IsVerified = false,
					GuidBoardGame = boardGameGuid,
					Created = DateTime.Now,
					AddedBy = userID,
				};
				await _db.AddBoardGame(newBoardGame);
				_logger.LogInformation(LoggingEvents.CreateNewGameSession, "Add new BoardGame to Database is successfully");

				boardGameID = _db.BoardGames.Where(x => x.GuidBoardGame == newBoardGame.GuidBoardGame).Select(x => x.ID).FirstOrDefault();
			}

			var gameSession = new GameSession
			{
				City = model.City,
				Address = model.Address,
				Name = model.Name,
				Description = model.Description,
				TimeStart = model.TimeStart,
				TimeEnd = model.TimeEnd,
				IsCanceled = false,
				Slots = model.Slots,
				SlotsFree = model.Slots - 1,
				UserAdminID = userAdmin.Id,
				Players = new List<GameSessionApplicationUser>() {
					new GameSessionApplicationUser
						{
							ApplicationUser = userAdmin,
							ApplicationUserID = userAdmin.Id
						}
				},
				Created = DateTime.Now,
			};

			int gameSessionId = await _db.AddGameSession(gameSession);
			_logger.LogInformation(LoggingEvents.CreateNewGameSession, "Create new GameSession is successfully");

			var gsBoardGame = new GameSessionBoardGame()
			{
				GameSessionID = gameSession.ID,
				BoardGameID = boardGameID
			};
			await _db.AddBoardGameToGameSession(gsBoardGame);
			_logger.LogInformation(LoggingEvents.CreateNewGameSession, "Add board games to new GameSession is successfully");

			var userProfile = await _db.Users.GetUserProfile(userID);
			await _db.UpdateNumberGamesSessionCreated(userProfile);

			return gameSessionId;

		}
		#endregion

		#region JoinGameSession

		public async Task<SingleGameSessionViewModel> JoinToCurrentGameSession(int gameSessionID, ClaimsPrincipal user)
		{
			string userID = user.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault().Value;
			var currentUser = _db.Users.GetUserAndProfileById(userID);

			var currentGameSession = await _db.GameSessions.GetActiveGameSessionByIdAsync(gameSessionID);

			List<ApplicationUser> gameSessionPlayers = _db.GameSessionApplicationUsers.GetGameSessionPlayers(currentGameSession);
			var adminEvent = _db.Users.GetEventAdmin(currentGameSession);

			if (currentGameSession.SlotsFree == 0)
			{
				var errorMessage = "Slots full";
				throw new RequestException(HttpStatusCode.BadRequest, errorMessage, LoggingEvents.JoinSingleGameSession);
			}
			else if (gameSessionPlayers.Contains(currentUser) || adminEvent == currentUser)
			{
				var errorMessage = "You are already in game";
				throw new RequestException(HttpStatusCode.BadRequest, errorMessage, LoggingEvents.JoinSingleGameSession);
			}
			else
			{
				var gsAppUser = new GameSessionApplicationUser
				{
					ApplicationUserID = userID,
					GameSessionID = currentGameSession.ID,
				};


				await _db.JoinGameSession(gsAppUser, currentGameSession);
				_logger.LogInformation(LoggingEvents.CreateNewGameSession, "Join to GameSession is successfully");

				await _db.UpdateNumberGamesJoined(currentUser.Profile);

				var singleGameSessionAfterJoin = await GetSingleGameSessionAsync(currentGameSession.ID, user);
				return singleGameSessionAfterJoin;
			}
		}
		#endregion

		#region LeaveGameSession

		public async Task<SingleGameSessionViewModel> LeaveCurrentGameSession(int gameSessionID, ClaimsPrincipal user)
		{
			string userID = user.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault().Value;
			var currentUser = _db.Users.GetUserAndProfileById(userID);

			var currentGameSession = await _db.GameSessions.GetActiveGameSessionByIdAsync(gameSessionID);

			List<ApplicationUser> gameSessionPlayers = _db.GameSessionApplicationUsers.GetGameSessionPlayers(currentGameSession);
			var adminEvent = _db.Users.GetEventAdmin(currentGameSession);

			if (adminEvent == currentUser)
			{
				var errorMessage = "You are event administrator. U cant leave current game";
				throw new RequestException(HttpStatusCode.BadRequest, errorMessage, LoggingEvents.LeaveSingleGameSession);
			}
			else if (!gameSessionPlayers.Contains(currentUser))
			{
				var errorMessage = "You are not in game. U cant leave current game";
				throw new RequestException(HttpStatusCode.BadRequest, errorMessage, LoggingEvents.LeaveSingleGameSession);
			}

			else
			{
				var gameSesAppUser = _db.GameSessionApplicationUsers.GetGameSessionAppUser(currentGameSession.ID, userID);

				await _db.LeaveGameSession(gameSesAppUser, currentGameSession);

				_logger.LogInformation(LoggingEvents.LeaveSingleGameSession, "Leave GameSession successfully");


				await _db.UpdateNumberGamesLeft(currentUser.Profile);

				var singleGameSessionAfterJoin = await GetSingleGameSessionAsync(currentGameSession.ID, user);

				return singleGameSessionAfterJoin;
			}
		}
		#endregion

		#region SearchGameSession

		public async Task<List<GameSessionViewModel>> SearchGameSessions(SearchGameSessionsViewModel model)
		{
			var searchParameter = new SearchParameters()
			{
				GameSessionName = model.SearchGameSessionName
			};

			if (model.SearchGameSessionDateFrom.HasValue)
			{
				searchParameter.StartDate = model.SearchGameSessionDateFrom.Value;
			}
			if (model.SearchGameSessionDateTo.HasValue)
			{
				searchParameter.EndDate = model.SearchGameSessionDateTo.Value;
			}
			var gameSessionList = await _db.GameSessions.SearchGameSession(searchParameter);

			var gameSessionViewModels = GameSessionMethods.GetListGameSessionsViewModel(gameSessionList.Where(x=>x.IsCanceled == false).ToList());

			_logger.LogInformation(LoggingEvents.GetGameSessionList, "Returned {Count} GameSessions", gameSessionViewModels.Count());
			return gameSessionViewModels;
		}
		#endregion

		#region DeletePlayerFromGameByAdmin
		public async Task<SingleGameSessionViewModel> DeletePlayerFromCurrentGameByAdmin(int gameSessionID, string userID, ClaimsPrincipal userAdminGS)
		{
			//player (admin) who want to delete player from GS
			string adminGS = userAdminGS.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault().Value;

			var currentGameSession = await _db.GameSessions.GetActiveGameSessionByIdAsync(gameSessionID);

			var admin = _db.Users.GetEventAdmin(currentGameSession).Id;

			if (adminGS != admin)
			{
				var errorMessage = "You are not admin of current Game Session !";
				throw new RequestException(HttpStatusCode.BadRequest, errorMessage, LoggingEvents.DeletePlayerFromCurrentGameByAdmin);
			}
			else
			{
				var gameSesionAppUser = _db.GameSessionApplicationUsers.GetGameSessionAppUser(currentGameSession.ID, userID);

				await _db.LeaveGameSession(gameSesionAppUser, currentGameSession);
				_logger.LogInformation(LoggingEvents.DeletePlayerFromCurrentGameByAdmin, "Delete Player From Current Game By Admin is successfully");

				var userProfile = await _db.Users.GetUserProfile(userID);
				await _db.UpdateNumberOfGamesSessionYouWereKickedOut(userProfile);

				var singleGameSessionAfterJoin = await GetSingleGameSessionAsync(currentGameSession.ID, userAdminGS);
				return singleGameSessionAfterJoin;
			}
		}
		#endregion

		#region DeleteGameSessionByAdmin
		public async Task DeleteGameSessionByAdmin(int gameSessionId, ClaimsPrincipal User)
		{
			GameSession gameSession = await _db.GameSessions.GetActiveGameSessionByIdAsync(gameSessionId);

			string userID = User.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault().Value;

			string userAdminID = _db.Users.GetUserById(userID).Id;

			if (userAdminID != gameSession.UserAdminID)
			{
				var errorMessage = "You are not game session admin. U cant delete current game session";
				throw new RequestException(HttpStatusCode.BadRequest, errorMessage, LoggingEvents.DeleteGameSessionByAdmin);
			}
			await _db.RemoveGameSessionAsync(gameSession);

			var userProfile = await _db.Users.GetUserProfile(userID);
			await _db.UpdateNumberGamesSessionDeletedasAdmin(userProfile);
		}


		#endregion

		#region GetMyGameSessionList
		public async Task<List<GameSessionViewModel>> GetMyGameSessionListAsync(ClaimsPrincipal user)
		{
			var myGameSessionList = new List<GameSession>();

			string userID = user.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault().Value;

			var gameSessionList = await _db.GameSessions.GetAllGameSessionsAsync();
			foreach (var gs in gameSessionList)
			{
				var gsPlayersID = gs.Players.Select(x => x.ApplicationUserID).ToList();
				if (gsPlayersID.Contains(userID))
				{
					myGameSessionList.Add(gs);
				}
			}

			var gameSessionViewModels = GameSessionMethods.GetListGameSessionsViewModel(myGameSessionList);

			_logger.LogInformation(LoggingEvents.GetGameSessionList, "Returned {Count} GameSessions", gameSessionViewModels.Count());
			return gameSessionViewModels;
		}
		#endregion

		#region GetBoardGamesDictionary
		public List<BoardGameDictionaryViewModel> GetBoardGamesDictionary()
		{

			return _db.BoardGames.Where(x=>x.IsVerified == true).Select(x => new BoardGameDictionaryViewModel() { Value = x.ID, Label = x.Name, ImageBoardGame = x.ImageBoardGame }).OrderBy(x=>x.Label).ToList();
		}
		#endregion

	}
}
