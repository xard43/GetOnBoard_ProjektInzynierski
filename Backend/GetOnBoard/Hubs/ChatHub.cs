using GetOnBoard.DAL;
using GetOnBoard.DAL.Repositories;
using GetOnBoard.ViewModels.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GetOnBoard.Chat.Hubs
{
	[Authorize]
	public class ChatHub : Hub
	{
		private readonly IChatRepository _chat;
		private readonly GetOnBoardDbContext _db;

		public ChatHub(IChatRepository chat, GetOnBoardDbContext db)
		{
			_chat = chat;
			_db = db;
		}
		public async Task SendMessage(string messageContent, int gameSessionId)
		{
			string userName = Context.User.FindFirstValue(ClaimTypes.Name);

			var avatar = await _db.Profiles.Where(x => x.ApplicationUserID == Context.UserIdentifier).Select(x => x.Avatar).FirstOrDefaultAsync();
				
			var message = new Message()
			{
				Author = userName,
				MessageContent = messageContent,
				SendTime = DateTime.Now,
				Avatar = avatar
			};
			await _chat.CreateNewMessageItemAsync(message, gameSessionId);
			await Clients.Group(gameSessionId.ToString()).SendAsync("ReceiveMessage", message);
		}

		public async Task EnterGame(int gameSessionID)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, gameSessionID.ToString());

			var history = await _chat.GetAllMessagesAsync(gameSessionID);
			await Clients.Client(Context.ConnectionId).SendAsync("History", history);
		}

		public async Task LeaveGame(int gameSessionID)
		{
			await Clients.Group(gameSessionID.ToString()).SendAsync("LeaveGame", gameSessionID.ToString());
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameSessionID.ToString());
		}
	}
}
