using GetOnBoard.Logging;
using GetOnBoard.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Extensions
{
	public static class GameSessionApplicationUsersExtensions
	{
		public static List<ApplicationUser> GetGameSessionPlayers(this DbSet<GameSessionApplicationUser> source, GameSession gameSession)
		{
			return source.Where(x => x.GameSession == gameSession)
						.Include(x => x.ApplicationUser)
						.Select(x => x.ApplicationUser)
						.Include(x => x.Profile)
						.ToList();
		}
		//get single user from GameSession from GameSessionApplicationUser table
		public static GameSessionApplicationUser GetGameSessionAppUser(this DbSet<GameSessionApplicationUser> source, int gameSessionID, string userID)
		{
			var gameSesAppUser = source.Where(x => x.GameSessionID == gameSessionID && x.ApplicationUserID == userID).FirstOrDefault();
			if (gameSesAppUser == null)
				throw new RequestException(HttpStatusCode.BadRequest, "Could not find user in current game session", LoggingEvents.GetUserFromGSisNull);

			return gameSesAppUser;
		}

		//get list of users from GameSession from GameSessionApplicationUser table
		public static List<GameSessionApplicationUser> GetGameSessionAllApplicationUser(this DbSet<GameSessionApplicationUser> source, GameSession gameSession)
		{
			return source.Where(x => x.GameSession == gameSession).ToList();
		}

		public static async Task RemoveGameSessionApplicationUserAsync(this GetOnBoardDbContext source, GameSessionApplicationUser gameSessionAppUser)
		{
			source.GameSessionApplicationUsers.Remove(gameSessionAppUser);
			await source.SaveChangesAsync();
		}
	}
}
