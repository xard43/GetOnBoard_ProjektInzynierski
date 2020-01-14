using GetOnBoard.DAL.Repositories;
using GetOnBoard.Logging;
using GetOnBoard.Models;
using GetOnBoard.Models.ViewModels.Profile;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Extensions
{
	public static class DbUserProfileExtensions
	{
		public static Task<UserProfile> GetUserProfile(this DbSet<ApplicationUser> source, string userID)
		{
			var userProfile = source.Where(x => x.Id == userID).Include(x => x.Profile).Select(x => x.Profile).
						FirstOrDefaultAsync();
			if (userProfile == null)
			{
				var errorMessage = "FAIL: User profile not found in the database";
				throw new RequestException(HttpStatusCode.BadRequest, errorMessage, LoggingEvents.GetUserProfile);
			}
			return userProfile;
		}

		public static async Task UpdateDataInUserProfile(this GetOnBoardDbContext source, UserProfile userProfile, UpdateProfileViewModel model, string boardGameImage)
		{
			userProfile.FirstName = model.FirstName;
			userProfile.LastName = model.LastName;
			userProfile.City = model.City;
			userProfile.Description = model.Description;
			userProfile.Avatar = boardGameImage;
			source.UserProfiles.Update(userProfile);
			await source.SaveChangesAsync();
		}
		
	}
}
