using System;

namespace GetOnBoard.ViewModels
{
	public class JWTUserTokenViewModel
	{
		public JWTUserTokenViewModel(string token, DateTime expiration, string refreshToken)
		{
			Token = token ?? throw new ArgumentNullException(nameof(token));
			Expiration = expiration;
			RefreshToken = refreshToken;
		}

		public string Token { get; set; }
		public DateTime Expiration { get; set; }
		public string RefreshToken { get; set; }
	}
}
