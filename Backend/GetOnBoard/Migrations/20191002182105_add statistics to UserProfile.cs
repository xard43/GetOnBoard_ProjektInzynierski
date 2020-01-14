using Microsoft.EntityFrameworkCore.Migrations;

namespace GetOnBoard.Migrations
{
    public partial class addstatisticstoUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberGamesJoined",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberGamesLeft",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberGamesSessionCreated",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberGamesSessionDeletedasAdmin",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberGamesJoined",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "NumberGamesLeft",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "NumberGamesSessionCreated",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "NumberGamesSessionDeletedasAdmin",
                table: "UserProfile");
        }
    }
}
