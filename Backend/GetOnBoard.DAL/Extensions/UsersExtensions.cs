using GetOnBoard.Logging;
using GetOnBoard.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Extensions
{
	public static class DbUsersExtensions
	{
		public static async Task<string> GetAdminAvatarAsync(this DbSet<ApplicationUser> source, GameSession gameSession)
		{
			return await source.Where(x => x.Id == gameSession.UserAdminID).
						Include(x => x.Profile).
						Select(x => x.Profile.Avatar).
						FirstOrDefaultAsync();
		}
		public static ApplicationUser GetUserById(this DbSet<ApplicationUser> source, string userID)
		{
			var user = source.Where(x => x.Id == userID).
						FirstOrDefault();
			if (user == null)
			{
				var errorMessage = "FAIL: User not found in the database";
				throw new RequestException(HttpStatusCode.BadRequest, errorMessage, LoggingEvents.GetUserById);
			}
			return user;
		}

		public static ApplicationUser GetUserAndProfileById(this DbSet<ApplicationUser> source, string userID)
		{
			var user = source.Where(x => x.Id == userID).Include(x=>x.Profile).
						FirstOrDefault();
			if (user == null)
			{
				var errorMessage = "FAIL: User not found in the database";
				throw new RequestException(HttpStatusCode.BadRequest, errorMessage, LoggingEvents.GetUserById);
			}
			return user;
		}

		public static ApplicationUser GetEventAdmin(this DbSet<ApplicationUser> source, GameSession gameSession)
		{
			return source.Where(x => x.Id == gameSession.UserAdminID).Include(x => x.Profile).FirstOrDefault();
		}
		
	}
}
