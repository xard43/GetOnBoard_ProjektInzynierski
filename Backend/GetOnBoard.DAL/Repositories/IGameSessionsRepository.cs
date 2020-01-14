using GetOnBoard.Models;
using GetOnBoard.Models.ViewModels.GameSession;
using GetOnBoard.ViewModels;
using GetOnBoard.ViewModels.GameSession;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Repositories
{
	public interface IGameSessionsRepository
	{
		Task<List<GameSessionViewModel>> GetGameSessionListAsync();
		Task<List<GameSessionViewModel>> SearchGameSessions(SearchGameSessionsViewModel model);

		Task<SingleGameSessionViewModel> GetSingleGameSessionAsync(int gameSessionID, ClaimsPrincipal user);

		Task<int> CreateNewGameSession(CreateNewGSViewModel model, ClaimsPrincipal user);

		Task<SingleGameSessionViewModel> JoinToCurrentGameSession(int gameSessionID, ClaimsPrincipal user);
		Task<SingleGameSessionViewModel> LeaveCurrentGameSession(int gameSessionID, ClaimsPrincipal user);

		Task<SingleGameSessionViewModel> DeletePlayerFromCurrentGameByAdmin(int gameSessionID, string userID, ClaimsPrincipal userAdminGS);

		Task DeleteGameSessionByAdmin(int gameSessionID, ClaimsPrincipal user);

		Task<List<GameSessionViewModel>> GetMyGameSessionListAsync(ClaimsPrincipal user);

		List<BoardGameDictionaryViewModel> GetBoardGamesDictionary();
	}
}
