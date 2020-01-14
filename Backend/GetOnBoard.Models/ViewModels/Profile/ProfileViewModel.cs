using System;
using System.Collections.Generic;
using System.Text;

namespace GetOnBoard.Models.ViewModels.Profile
{
	public class ProfileViewModel
	{
		public string UserID { get; set; }
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Avatar { get; set; }
		public string City { get; set; }
		public string Description { get; set; }
		public int NumberOfGamesSessionCreated { get; set; }
		public int NumberOfGamesSessionDeletedasAdmin { get; set; }
		public int NumberOfGamesSessionLeft { get; set; }
		public int NumberOfGamesSessionJoined { get; set; }
		public int NumberOfGamesSessionYouWereKickedOut { get; set; }
		public bool IsItMyProfile { get; set; }
	}
}
