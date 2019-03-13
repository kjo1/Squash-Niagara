using Microsoft.EntityFrameworkCore.Migrations;

namespace SN_BNB.Data.SNMigrations
{
    public partial class teambiofix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TeamBio",
                schema: "SN",
                table: "Teams",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TeamBio",
                schema: "SN",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
