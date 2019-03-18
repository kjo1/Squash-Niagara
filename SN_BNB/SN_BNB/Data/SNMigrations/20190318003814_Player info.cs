using Microsoft.EntityFrameworkCore.Migrations;

namespace SN_BNB.Data.SNMigrations
{
    public partial class Playerinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                schema: "SN",
                table: "Players",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Against",
                schema: "SN",
                table: "Players",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "For",
                schema: "SN",
                table: "Players",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Played",
                schema: "SN",
                table: "Players",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                schema: "SN",
                table: "Players",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Against",
                schema: "SN",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "For",
                schema: "SN",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Played",
                schema: "SN",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Points",
                schema: "SN",
                table: "Players");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                schema: "SN",
                table: "Players",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
