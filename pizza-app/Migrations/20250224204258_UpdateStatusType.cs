using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pizza_app.Migrations
{
    public partial class UpdateStatusType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Commandes",
                type: "TEXT",
                nullable: false,
                defaultValue: "EnCours",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "En attente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Commandes",
                type: "TEXT",
                nullable: false,
                defaultValue: "En attente",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "EnCours");
        }
    }
}
