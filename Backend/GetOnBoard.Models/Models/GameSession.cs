using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GetOnBoard.Models
{
	[JsonObject(IsReference = true)]
	public class GameSession
	{
		[Required]
		public int ID { get; set; }
		public string City { get; set; }
		//TODO: Move Address to class
		public string Address { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime TimeStart { get; set; }
		public DateTime TimeEnd { get; set; }
		public bool IsCanceled { get; set; }
		public int Slots { get; set; }
		public int SlotsFree { get; set; }
		public string UserAdminID { get; set; }
		public DateTime Created { get; set; }

		public virtual IEnumerable<GameSessionApplicationUser> Players { get; set; }
		public virtual IEnumerable<GameSessionBoardGame> BoardGames { get; set; }
		public virtual IEnumerable<MessageEntity> Messages { get; set; }

	}
}
