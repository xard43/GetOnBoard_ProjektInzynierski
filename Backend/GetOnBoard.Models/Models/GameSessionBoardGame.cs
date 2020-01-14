namespace GetOnBoard.Models
{
	public class GameSessionBoardGame
	{
		public int ID { get; set; }

		public virtual GameSession GameSession { get; set; }
		public int GameSessionID { get; set; }
		public virtual BoardGame BoardGame { get; set; }
		public int BoardGameID { get; set; }

	}
}
