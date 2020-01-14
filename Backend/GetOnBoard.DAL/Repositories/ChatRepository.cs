using GetOnBoard.Models;
using GetOnBoard.ViewModels.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Repositories
{
	public class ChatRepository : IChatRepository
	{
		private readonly ILogger<ChatRepository> _logger;
		private readonly GetOnBoardDbContext _db;

		public ChatRepository(ILogger<ChatRepository> logger, GetOnBoardDbContext db)
		{
			_logger = logger;
			_db = db;
		}

		public async Task CreateNewMessageItemAsync(Message message, int gameSessionId)
		{
			await _db.MessageEntities.AddAsync(new MessageEntity()
			{
				Author = message.Author,
				Message = message.MessageContent,
				GameSessionId = gameSessionId,
				SendTime = message.SendTime,
				Avatar = message.Avatar
			});
			await _db.SaveChangesAsync();
			_logger.LogInformation($"New message added to GameSession: {gameSessionId}");
		}

		public async Task<List<Message>> GetAllMessagesAsync(int gameSessionId)
		{
			return await _db.MessageEntities.
				Where(it => it.GameSessionId == gameSessionId).
				Select(it => new Message()
				{
					Author = it.Author,
					MessageContent = it.Message,
					SendTime = it.SendTime,
					Avatar = it.Avatar
				}).
				ToListAsync();
		}
	}
}
