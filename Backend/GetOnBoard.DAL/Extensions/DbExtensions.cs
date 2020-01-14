using GetOnBoard.Models;
using GetOnBoard.Models.Models;
using GetOnBoard.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Extensions
{
	public static class DbExtensions
	{
		public static async Task<int> AddGameSession(this GetOnBoardDbContext source, GameSession gameSession)
		{
			source.GameSessions.Add(gameSession);
			await source.SaveChangesAsync();
			return gameSession.ID;
		}

		public static async Task AddBoardGameToGameSession(this GetOnBoardDbContext source, GameSessionBoardGame gameSessionBG)
		{
			source.GameSessionBoardGames.Add(gameSessionBG);
			await source.SaveChangesAsync();
		}

		public static async Task AddBoardGame(this GetOnBoardDbContext source, BoardGame boardGame)
		{
			source.BoardGames.Add(boardGame);
			await source.SaveChangesAsync();
		}

		public static async Task RemoveGameSessionAsync(this GetOnBoardDbContext source, GameSession gameSession)
		{
            gameSession.IsCanceled = true;
            source.GameSessions.Update(gameSession);
            await source.SaveChangesAsync();
		}

		public static async Task JoinGameSession(this GetOnBoardDbContext source, GameSessionApplicationUser gSAppUser,GameSession gameSession)
		{
			source.GameSessionApplicationUsers.Add(gSAppUser);
			await source.SaveChangesAsync();
			await source.UpdateFreeSlotsInGS(gameSession, 1);
		}

		public static async Task LeaveGameSession(this GetOnBoardDbContext source, GameSessionApplicationUser gSAppUser, GameSession gameSession)
		{
			source.GameSessionApplicationUsers.Remove(gSAppUser);
			await source.SaveChangesAsync();
			await source.UpdateFreeSlotsInGS(gameSession, -1);
		}

		public static async Task UpdateFreeSlotsInGS(this GetOnBoardDbContext source, GameSession gameSession, int count)
		{
			gameSession.SlotsFree = gameSession.SlotsFree + count;
			source.GameSessions.Update(gameSession);
			await source.SaveChangesAsync();
		}

		public static async Task UpdateNumberGamesSessionCreated(this GetOnBoardDbContext source, UserProfile userProfile)
		{
			userProfile.NumberOfGamesSessionCreated = userProfile.NumberOfGamesSessionCreated + 1;
			await source.SaveChangesAsync();
		}

		public static async Task UpdateNumberGamesSessionDeletedasAdmin(this GetOnBoardDbContext source, UserProfile userProfile)
		{
			userProfile.NumberOfGamesSessionDeletedasAdmin = userProfile.NumberOfGamesSessionDeletedasAdmin + 1;
			await source.SaveChangesAsync();
		}

		public static async Task UpdateNumberGamesLeft(this GetOnBoardDbContext source, UserProfile userProfile)
		{
			userProfile.NumberOfGamesSessionLeft = userProfile.NumberOfGamesSessionLeft + 1;
			await source.SaveChangesAsync();
		}

		public static async Task UpdateNumberGamesJoined(this GetOnBoardDbContext source, UserProfile userProfile)
		{
			userProfile.NumberOfGamesSessionJoined = userProfile.NumberOfGamesSessionJoined + 1;
			await source.SaveChangesAsync();
		}

		public static async Task UpdateNumberOfGamesSessionYouWereKickedOut(this GetOnBoardDbContext source, UserProfile userProfile)
		{
			userProfile.NumberOfGamesSessionYouWereKickedOut = userProfile.NumberOfGamesSessionYouWereKickedOut + 1;
			await source.SaveChangesAsync();
		}

		public static async Task AddMessageFromUser(this GetOnBoardDbContext source, Contact contact )
		{
			source.Contact.Add(contact);
			
			await source.SaveChangesAsync();
		}

	}
}
