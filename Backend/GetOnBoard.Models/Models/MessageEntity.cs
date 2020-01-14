using System;
using System.Collections.Generic;
using System.Text;

namespace GetOnBoard.Models
{
	public class MessageEntity
	{
		public long Id { get; set; }
		public string Message { get; set; }
		public DateTime SendTime { get; set; }
		public string Author { get; set; }
		public int GameSessionId { get; set; }
		public GameSession GameSession { get; set; }
		public string Avatar { get; set; }
	}
}
