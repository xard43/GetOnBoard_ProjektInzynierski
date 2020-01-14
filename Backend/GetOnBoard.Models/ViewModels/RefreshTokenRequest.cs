using System;
using System.Collections.Generic;
using System.Text;

namespace GetOnBoard.Models.ViewModels
{
	public class RefreshTokenRequest
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}
}
