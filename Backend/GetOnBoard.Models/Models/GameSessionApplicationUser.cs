using Newtonsoft.Json;

namespace GetOnBoard.Models
{
	[JsonObject(IsReference = true)]
	public class GameSessionApplicationUser
	{
		public int ID { get; set; }

		public virtual GameSession GameSession { get; set; }
		public int GameSessionID { get; set; }
		public virtual ApplicationUser ApplicationUser { get; set; }
		public string ApplicationUserID { get; set; }
		//public bool IsOwner { get; set; }
	}
}
