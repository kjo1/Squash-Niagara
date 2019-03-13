using Microsoft.EntityFrameworkCore.Migrations;

namespace SN_BNB.Data.SNMigrations
{
    public partial class bioaddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamBio",
                schema: "SN",
                table: "Teams",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamBio",
                schema: "SN",
                table: "Teams");
        }
    }
}
