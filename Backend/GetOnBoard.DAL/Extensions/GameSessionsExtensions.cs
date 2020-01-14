using GetOnBoard.Logging;
using GetOnBoard.Models;
using GetOnBoard.Models.ViewModels.GameSession;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Extensions
{
	public static class GameSessionsExtensions
	{
		public static async Task<List<GameSession>> GetAllGameSessionsAsync(this DbSet<GameSession> source)
		{
            return await source.Where(x => x.TimeStart >= DateTime.Now.AddMinutes(15) && x.IsCanceled == false)
				.Include(x => x.BoardGames)
				.ThenInclude(x => x.BoardGame)
				.Include(x => x.Players)
				.ThenInclude(x => x.ApplicationUser)
				.ThenInclude(x => x.Profile)
				.ToListAsync();
		}
		public static async Task<List<GameSession>> GetAllGamessesionWithBoardGamesAsync(this DbSet<GameSession> source)
		{
			return await source.Where(x => x.TimeStart >= DateTime.Now.AddMinutes(15) && x.IsCanceled == false).Include(x => x.Players).Include(x => x.BoardGames).ThenInclude(x => x.BoardGame).ToListAsync();
		}

		public static async Task<GameSession> GetActiveGameSessionByIdAsync(this DbSet<GameSession> source, int id)
		{
			GameSession gameSession = await source.FindAsync(id);
			if (gameSession == null)
			{
				var message = string.Format("GameSession with id {0} not found", id);
				throw new RequestException(HttpStatusCode.NotFound, message, LoggingEvents.GetActiveGameSessionByIdAsync);
			}
			else if (gameSession.TimeStart <= DateTime.Now.AddMinutes(15))
			{
				var message = string.Format("GameSession with id {0} is inactive", id);
				throw new RequestException(HttpStatusCode.BadRequest, message, LoggingEvents.GetActiveGameSessionByIdAsync);
			}
			else if (gameSession.IsCanceled)
			{
				var message = string.Format("GameSession with id {0} is canceled", id);
				throw new RequestException(HttpStatusCode.BadRequest, message, LoggingEvents.GetActiveGameSessionByIdAsync);
			}
			return gameSession;
		}

		public static async Task<List<GameSession>> SearchGameSession(this DbSet<GameSession> source, SearchParameters searchParameters)
		{
			var query = source.Where(x=> x.TimeStart >= DateTime.Now.AddMinutes(15)).Select(it => it);
			if (!string.IsNullOrEmpty(searchParameters.GameSessionName))
			{
				query = query.Where(it => it.Name.ToLowerInvariant().Contains(searchParameters.GameSessionName.ToLowerInvariant()));
			}
			if (searchParameters.StartDate.HasValue)
			{
				query = query.Where(it => it.TimeStart >= searchParameters.StartDate);
			}
			if (searchParameters.EndDate.HasValue)
			{
				query = query.Where(it => it.TimeStart <= searchParameters.EndDate);
			}
			return await query.Include(x => x.BoardGames)
				.ThenInclude(x => x.BoardGame)
				.Include(x => x.Players)
				.ThenInclude(x => x.ApplicationUser)
				.ThenInclude(x => x.Profile)
				.ToListAsync();
		}
	}
}
