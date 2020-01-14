using Microsoft.EntityFrameworkCore.Migrations;

namespace GetOnBoard.Migrations
{
    public partial class addavatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "MessageEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "MessageEntities");
        }
    }
}
