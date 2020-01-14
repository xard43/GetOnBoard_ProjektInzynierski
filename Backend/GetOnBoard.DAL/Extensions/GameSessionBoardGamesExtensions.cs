using GetOnBoard.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Extensions
{
	public static class GameSessionBoardGamesExtensions
	{
		public static List<BoardGame> GetBoardGamesFromGameSession(this DbSet<GameSessionBoardGame> source, GameSession gameSession)
		{
			return source.Where(x => x.GameSession == gameSession).
						Include(x => x.BoardGame).
						Select(x => x.BoardGame).
						ToList();
		}

		public static List<GameSessionBoardGame> GetGameSessionBoardGames(this DbSet<GameSessionBoardGame> source, GameSession gameSession)
		{
			return source.Where(x => x.GameSession == gameSession).ToList();
		}

		public static async Task RemoveGameSessionBoardGamesAsync(this GetOnBoardDbContext source, GameSessionBoardGame gameSessionBG)
		{
			source.GameSessionBoardGames.Remove(gameSessionBG);
			await source.SaveChangesAsync();
		}
	}
}
