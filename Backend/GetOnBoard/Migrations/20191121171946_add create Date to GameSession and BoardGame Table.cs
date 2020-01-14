using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GetOnBoard.Migrations
{
    public partial class addcreateDatetoGameSessionandBoardGameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "GameSessions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BoardGames",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "BoardGames");
        }
    }
}
