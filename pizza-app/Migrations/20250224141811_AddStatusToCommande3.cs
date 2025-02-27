using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pizza_app.Migrations
{
    public partial class AddStatusToCommande3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Commandes",
                type: "TEXT",
                nullable: false,
                defaultValue: "En attente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Commandes");
        }
    }
}
