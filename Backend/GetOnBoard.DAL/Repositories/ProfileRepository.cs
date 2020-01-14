using GetOnBoard.DAL.Extensions;
using GetOnBoard.Models;
using Microsoft.Extensions.Logging;
using GetOnBoard.Models.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using System.IO;

namespace GetOnBoard.DAL.Repositories
{
	public class ProfileRepository : IProfileRepository
	{
		private readonly ILogger<ProfileRepository> _logger;
		private readonly GetOnBoardDbContext _db;
		private readonly IBlobStorageRepository _blobStorage;

		public ProfileRepository(ILogger<ProfileRepository> logger, GetOnBoardDbContext db, IBlobStorageRepository blobStorage)
		{
			_logger = logger;
			_db = db;
			_blobStorage = blobStorage;
		}
		#region GetUserProfile
		public ProfileViewModel GetUserProfile(string userProfileID, ClaimsPrincipal user)
		{
			string userClaimID = user.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault().Value;

			var userAndUserProfile = _db.Users.GetUserAndProfileById(userProfileID);

			bool isItUserClaimProfile = false;

			if (userAndUserProfile.Id == userClaimID)
			{
				isItUserClaimProfile = true;
			}


			return new ProfileViewModel()
			{
				UserID = userAndUserProfile.Id,
				UserName = userAndUserProfile.UserName,
				Avatar = userAndUserProfile.Profile.Avatar,
				City = userAndUserProfile.Profile.City,
				Description = userAndUserProfile.Profile.Description,
				FirstName = userAndUserProfile.Profile.FirstName,
				LastName = userAndUserProfile.Profile.LastName,
				NumberOfGamesSessionCreated = userAndUserProfile.Profile.NumberOfGamesSessionCreated,
				NumberOfGamesSessionDeletedasAdmin = userAndUserProfile.Profile.NumberOfGamesSessionDeletedasAdmin,
				NumberOfGamesSessionJoined = userAndUserProfile.Profile.NumberOfGamesSessionJoined,
				NumberOfGamesSessionLeft = userAndUserProfile.Profile.NumberOfGamesSessionLeft,
				NumberOfGamesSessionYouWereKickedOut = userAndUserProfile.Profile.NumberOfGamesSessionYouWereKickedOut,
				IsItMyProfile = isItUserClaimProfile
			};

		}
		#endregion

		#region UpdateUserProfile
		public async Task UpdateDataInProfile(UpdateProfileViewModel model)
		{
			var userProfile = await _db.Users.GetUserProfile(model.UserID);
			string boardGameImage = userProfile.Avatar;
			if (!string.IsNullOrEmpty(model.Avatar))
			{
				
				byte[] imageBoardGameBytes = Convert.FromBase64String(model.Avatar);
				Stream stream = new MemoryStream(imageBoardGameBytes);
				await _blobStorage.Save(stream, $"{model.UserID}.jpg");

				boardGameImage = _blobStorage.Load($"{model.UserID}.jpg").Uri.AbsoluteUri;

			}



			await _db.UpdateDataInUserProfile(userProfile, model, boardGameImage);
		}
		#endregion
	}
}
