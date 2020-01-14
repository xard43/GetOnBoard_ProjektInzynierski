using Microsoft.EntityFrameworkCore.Migrations;

namespace GetOnBoard.Migrations
{
    public partial class addaddedByDatetoBoardGameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "BoardGames",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "BoardGames");
        }
    }
}
