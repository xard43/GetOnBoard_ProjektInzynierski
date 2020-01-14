using GetOnBoard.DAL;
using GetOnBoard.DAL.Repositories;
using GetOnBoard.Models;
using GetOnBoard.Models.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GetOnBoard.Controllers
{
	[Authorize]
	public class ImageController : Controller
	{
		private readonly GetOnBoardDbContext _dbContext;
		private readonly ILogger _logger;
		private readonly IBlobStorageRepository _blobStorage;


		public ImageController(GetOnBoardDbContext dbContext, ILogger<ImageController> logger, IBlobStorageRepository blobStorage)
		{
			_dbContext = dbContext;
			_logger = logger;
			_blobStorage = blobStorage;
		}

		
		[HttpPost]
		[Route("upload")]
		[SwaggerResponse((int)HttpStatusCode.Accepted, "Returns profile with setted Avatar value")]
		public async Task<IActionResult> Upload([FromBody] UpdateImageInProfile model)
		{
			string userID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			if (userID == null)
			{
				throw new Exception("Uknown User");
			}

			//using (Stream stream = file.OpenReadStream())
			//{
			//	await _blobStorage.Save(stream, $"{userID}.jpg");
			//}
			UserProfile profile = _dbContext.UserProfiles.FirstOrDefault(x => x.ApplicationUserID == userID) ?? new UserProfile();

			byte[] imageBoardGameBytes = Convert.FromBase64String(model.Avatar);
			Stream stream = new MemoryStream(imageBoardGameBytes);
			await _blobStorage.Save(stream, $"{userID}.jpg");
			
			profile.Avatar = _blobStorage.Load($"{userID}.jpg").Uri.AbsoluteUri;

			await _dbContext.SaveChangesAsync();
			return StatusCode((int)HttpStatusCode.Accepted, Json(profile));
		}

		[Route("GetImage")]
		[HttpGet]
		public async Task<IActionResult> GetImage(string fileName)
		{
			var blockBlob = _blobStorage.Load(fileName);
			var stream = await blockBlob.OpenReadAsync();
			// This usage of File() always triggers the browser to perform a file download.
			// We always use "application/octet-stream" as the content type because we don't record
			// any information about content type from the user when they upload a file.
			return File(stream, "image/png", fileName);
		}
	}
}