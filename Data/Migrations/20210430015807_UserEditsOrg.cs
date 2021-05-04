using Microsoft.EntityFrameworkCore.Migrations;

namespace HUD.Data.Migrations
{
    public partial class UserEditsOrg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Organization",
                table: "AspNetUsers",
                newName: "OrganizationPrefix");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrganizationPrefix",
                table: "AspNetUsers",
                newName: "Organization");
        }
    }
}
