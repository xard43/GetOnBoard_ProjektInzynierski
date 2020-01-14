using GetOnBoard.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Repositories
{
	public interface IChatRepository
	{
		Task CreateNewMessageItemAsync(Message message, int gameSessionId);
		Task<List<Message>> GetAllMessagesAsync(int gameSessionId);
	}
}
