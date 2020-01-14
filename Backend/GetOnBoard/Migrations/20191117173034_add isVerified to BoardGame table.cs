using Microsoft.EntityFrameworkCore.Migrations;

namespace GetOnBoard.Migrations
{
    public partial class addisVerifiedtoBoardGametable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IsVerified",
                table: "BoardGames",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "BoardGames");
        }
    }
}
