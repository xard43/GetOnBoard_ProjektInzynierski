using GetOnBoard.Models;
using GetOnBoard.ViewModels.GameSession;
using System;
using System.Collections.Generic;

namespace GetOnBoard.ViewModels
{
    public class SingleGameSessionViewModel
    {
        public int ID { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsCurrentUserInGame { get; set; }
        public string Description { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int Slots { get; set; }
        public int SlotsFree { get; set; }
        public string AdminID { get; set; }
        public string AdminAvatar { get; set; }
        public List<PlayerSingleGSViewModel> Players { get; set; }
        public List<BoardGameSingleGSViewModel> BoardGamesEvent { get; set; }
        //public List<BoardGameSingleGSViewModel> BoardGamesEvent = new List<BoardGameSingleGSViewModel>();
        //public List<PlayerSingleGSViewModel> Players = new List<PlayerSingleGSViewModel>();
        
    }
}
