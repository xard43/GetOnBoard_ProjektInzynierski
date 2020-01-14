using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetOnBoard.ViewModels.GameSession
{
    public class BoardGameSingleGSViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int? PlayersMin { get; set; }
        public int? PlayersMax { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
        public int? GameTimeMin { get; set; }
        public int? GameTimeMax { get; set; }
        public int? Age { get; set; }
        public string BoardGameAvatar { get; set; }
    }
}
