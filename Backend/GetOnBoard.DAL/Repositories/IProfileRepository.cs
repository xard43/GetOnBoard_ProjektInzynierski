using GetOnBoard.Models.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Repositories
{
	public interface IProfileRepository
	{
		ProfileViewModel GetUserProfile(string userID, ClaimsPrincipal user);
		Task UpdateDataInProfile(UpdateProfileViewModel model);
		
	}
}
