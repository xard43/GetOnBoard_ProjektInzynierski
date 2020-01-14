using System;
using System.Collections.Generic;
using System.Text;

namespace GetOnBoard.ViewModels.Chat
{
	public class Message
	{
		public string MessageContent { get; set; }
		public string Author { get; set; }
		public DateTime  SendTime { get; set; }
		public string Avatar { get; set; }
	}
}
