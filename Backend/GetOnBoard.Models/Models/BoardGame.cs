using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GetOnBoard.Models
{
	[JsonObject(IsReference = true)]
	public class BoardGame
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Categories { get; set; }
		public int? Age { get; set; }
		public int? PlayersMin { get; set; }
		public int? PlayersMax { get; set; }
		public int? GameTimeMin { get; set; }
		public int? GameTimeMax { get; set; }
		public int? ReleaseYear { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }
		public string ImageBoardGame { get; set; }
		public bool? IsVerified { get; set; }
		public Guid? GuidBoardGame { get; set; }
		public DateTime Created { get; set; }
		public string AddedBy { get; set; }
		
		public virtual IEnumerable<GameSessionBoardGame> GameSessions { get; set; }
	}
}
