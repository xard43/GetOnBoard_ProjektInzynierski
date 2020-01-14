using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace GetOnBoard.Chat.Hubs
{
	public class ChatHub : Hub
	{
		public ChatHub(GetOnBoardDbContext)
		public async Task SendMessage(string user, string message, int gameSessionId)
		{
			await Clients.Group(gameSessionId.ToString()).SendAsync("ReceiveMessage", user, message);
		}
	}
}
