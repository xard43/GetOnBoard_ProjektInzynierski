using System;
using System.Collections.Generic;
using System.Text;

namespace GetOnBoard.Models.ViewModels.Profile
{
	public class UpdateProfileViewModel
	{
		public string UserID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string City { get; set; }
		public string Description { get; set; }
		public string Avatar { get; set; }

	}
}
