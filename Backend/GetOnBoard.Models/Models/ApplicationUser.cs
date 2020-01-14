using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GetOnBoard.Models
{
	[JsonObject(IsReference = true)]
	public class ApplicationUser : IdentityUser
	{
		public bool? IsActivated { get; set; }
		public bool? IsBanned { get; set; }
		public virtual UserProfile Profile { get; set; }
		public virtual IEnumerable<GameSessionApplicationUser> GameSessions { get; set; }
	}
}
