using Microsoft.EntityFrameworkCore.Migrations;

namespace HUD.Data.Migrations
{
    public partial class AddedSettingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userSettings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserGuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ticketOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ticketsToShow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numTicketsPerStatus = table.Column<int>(type: "int", nullable: false),
                    numTickets = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userSettings", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userSettings");
        }
    }
}
