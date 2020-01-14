using Microsoft.EntityFrameworkCore.Migrations;

namespace GetOnBoard.Migrations
{
    public partial class Statistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberGamesSessionDeletedasAdmin",
                table: "UserProfile",
                newName: "NumberOfGamesSessionYouWereKickedOut");

            migrationBuilder.RenameColumn(
                name: "NumberGamesSessionCreated",
                table: "UserProfile",
                newName: "NumberOfGamesSessionLeft");

            migrationBuilder.RenameColumn(
                name: "NumberGamesLeft",
                table: "UserProfile",
                newName: "NumberOfGamesSessionJoined");

            migrationBuilder.RenameColumn(
                name: "NumberGamesJoined",
                table: "UserProfile",
                newName: "NumberOfGamesSessionDeletedasAdmin");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfGamesSessionCreated",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfGamesSessionCreated",
                table: "UserProfile");

            migrationBuilder.RenameColumn(
                name: "NumberOfGamesSessionYouWereKickedOut",
                table: "UserProfile",
                newName: "NumberGamesSessionDeletedasAdmin");

            migrationBuilder.RenameColumn(
                name: "NumberOfGamesSessionLeft",
                table: "UserProfile",
                newName: "NumberGamesSessionCreated");

            migrationBuilder.RenameColumn(
                name: "NumberOfGamesSessionJoined",
                table: "UserProfile",
                newName: "NumberGamesLeft");

            migrationBuilder.RenameColumn(
                name: "NumberOfGamesSessionDeletedasAdmin",
                table: "UserProfile",
                newName: "NumberGamesJoined");
        }
    }
}
