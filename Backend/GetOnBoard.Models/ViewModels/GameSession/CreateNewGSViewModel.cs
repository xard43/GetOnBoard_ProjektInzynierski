using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetOnBoard.ViewModels.GameSession
{
	public class CreateNewGSViewModel
	{
		public string City { get; set; }
		public string Address { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime TimeStart { get; set; }
		public DateTime TimeEnd { get; set; }
		public int Slots { get; set; }
		public int BoardGameID { get; set; }

		public bool IsCustomGame { get; set; }
		public string CustomGameName { get; set; }
		public string CustomGameImage { get; set; }

	}
}
