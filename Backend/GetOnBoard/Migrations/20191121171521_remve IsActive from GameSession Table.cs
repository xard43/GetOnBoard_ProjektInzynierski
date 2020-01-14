using Microsoft.EntityFrameworkCore.Migrations;

namespace GetOnBoard.Migrations
{
    public partial class remveIsActivefromGameSessionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "GameSessions");

            migrationBuilder.RenameColumn(
                name: "GuidGameSession",
                table: "BoardGames",
                newName: "GuidBoardGame");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GuidBoardGame",
                table: "BoardGames",
                newName: "GuidGameSession");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "GameSessions",
                nullable: false,
                defaultValue: false);
        }
    }
}
