using Newtonsoft.Json;

namespace GetOnBoard.Models
{
	[JsonObject(IsReference = true)]
	public class UserProfile
	{
		public int ID { get; set; }
		public string Avatar { get; set; }
		public string City { get; set; }
		public string Description { get; set; }
		public string Phone { get; set; }
		public string Phone_private { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ApplicationUserID { get; set; }
		public int NumberOfGamesSessionCreated { get; set; }
		public int NumberOfGamesSessionDeletedasAdmin { get; set; }
		public int NumberOfGamesSessionLeft { get; set; }
		public int NumberOfGamesSessionJoined { get; set; }
		public int NumberOfGamesSessionYouWereKickedOut{ get; set; }

		public virtual ApplicationUser ApplicationUser { get; set; }
	}
}