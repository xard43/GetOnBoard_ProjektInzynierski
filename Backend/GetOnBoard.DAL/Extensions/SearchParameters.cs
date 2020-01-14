using System;
using System.Collections.Generic;
using System.Text;

namespace GetOnBoard.DAL.Extensions
{
	public class SearchParameters
	{
		public string GameSessionName { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

	}
}
