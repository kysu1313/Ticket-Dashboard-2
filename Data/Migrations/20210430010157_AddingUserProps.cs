using Microsoft.EntityFrameworkCore.Migrations;

namespace HUD.Data.Migrations
{
    public partial class AddingUserProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userId",
                table: "AspNetUsers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "organization",
                table: "AspNetUsers",
                newName: "Organization");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetUsers",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Organization",
                table: "AspNetUsers",
                newName: "organization");
        }
    }
}
