using System;
using System.Collections.Generic;
using System.Text;

namespace GetOnBoard.Models.ViewModels.GameSession
{
	public class SearchGameSessionsViewModel
	{
		public string SearchGameSessionName { get; set; }
		public DateTime? SearchGameSessionDateFrom { get; set; }
		public DateTime? SearchGameSessionDateTo { get; set; }
	}
}
