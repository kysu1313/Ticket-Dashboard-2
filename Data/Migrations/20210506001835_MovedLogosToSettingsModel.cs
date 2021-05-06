using Microsoft.EntityFrameworkCore.Migrations;

namespace HUD.Data.Migrations
{
    public partial class MovedLogosToSettingsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LargeLogo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SmallLogo",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "LargeLogo",
                table: "userSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmallLogo",
                table: "userSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LargeLogo",
                table: "userSettings");

            migrationBuilder.DropColumn(
                name: "SmallLogo",
                table: "userSettings");

            migrationBuilder.AddColumn<string>(
                name: "LargeLogo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmallLogo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
