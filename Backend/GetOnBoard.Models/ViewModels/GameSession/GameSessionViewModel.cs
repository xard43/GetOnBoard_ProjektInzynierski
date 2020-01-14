using GetOnBoard.Models;
using GetOnBoard.Models.ViewModels.GameSession;
using System;
using System.Collections.Generic;

namespace GetOnBoard.ViewModels
{
	
	public class GameSessionViewModel
	{
		public int ID { get; set; }
		public string City { get; set; }
		public string Address { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime TimeStart { get; set; }
		public DateTime TimeEnd { get; set; }
		public int Slots { get; set; }
	    public int SlotsFree { get; set; }
		public string GameAvatar { get; set; }
		public string GameName { get; set; }
		public List<GetUserNameAndAvaterViewModel> Players { get; set; }

	}
}
