using GetOnBoard.DAL.Extensions;
using GetOnBoard.Logging;
using GetOnBoard.Models;
using GetOnBoard.ViewModels;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using GetOnBoard.Models.ViewModels.GameSession;

namespace GetOnBoard.DAL.Helper
{
	public static class GameSessionMethods
	{
		public static List<GameSessionViewModel> GetListGameSessionsViewModel(List<GameSession> gameSessionList)
		{
			
			var gameSessionViewModels = gameSessionList.
			AsParallel().
			Select(it =>
			new GameSessionViewModel()
			{
				Address = it.Address,
				City = it.City,
				Description = it.Description,
				GameAvatar = it.BoardGames.FirstOrDefault()?.BoardGame?.ImageBoardGame,
				GameName = it.BoardGames.FirstOrDefault()?.BoardGame?.Name,
				Players = it.Players.Select(x=>x.ApplicationUser?.Profile).Select(i => new GetUserNameAndAvaterViewModel() { UserName = i.ApplicationUser.UserName, Avatar = i.Avatar }).ToList(),
				ID = it.ID,
				Name = it.Name,
				Slots = it.Slots,
				SlotsFree = it.SlotsFree,
				TimeEnd = it.TimeEnd,
				TimeStart = it.TimeStart,
			}).OrderBy(x => x.TimeStart).ToList();

			return gameSessionViewModels;

		}
	}
}
