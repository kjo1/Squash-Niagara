using Microsoft.EntityFrameworkCore.Migrations;

namespace SN_BNB.Data.SNMigrations
{
    public partial class Summaryprop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamLosses",
                schema: "SN",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamWins",
                schema: "SN",
                table: "Teams");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamLosses",
                schema: "SN",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamWins",
                schema: "SN",
                table: "Teams",
                nullable: false,
                defaultValue: 0);
        }
    }
}
