using Microsoft.EntityFrameworkCore.Migrations;

namespace GetOnBoard.Migrations
{
    public partial class modificationBoradGametable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Score",
                table: "BoardGames",
                newName: "ReleaseYear");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "BoardGames",
                newName: "GameTimeMin");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "BoardGames",
                newName: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "BoardGames",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "BoardGames",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameTimeMax",
                table: "BoardGames",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "BoardGames");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "BoardGames");

            migrationBuilder.DropColumn(
                name: "GameTimeMax",
                table: "BoardGames");

            migrationBuilder.RenameColumn(
                name: "ReleaseYear",
                table: "BoardGames",
                newName: "Score");

            migrationBuilder.RenameColumn(
                name: "GameTimeMin",
                table: "BoardGames",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "Categories",
                table: "BoardGames",
                newName: "Category");
        }
    }
}
