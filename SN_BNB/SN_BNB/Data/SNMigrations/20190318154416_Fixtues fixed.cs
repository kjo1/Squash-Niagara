using Microsoft.EntityFrameworkCore.Migrations;

namespace SN_BNB.Data.SNMigrations
{
    public partial class Fixtuesfixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Teams_AwayTeamID",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Teams_HomeTeamID",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_AwayTeamID",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_HomeTeamID",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "AwayTeamID",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "HomeTeamID",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_idAwayTeam",
                schema: "SN",
                table: "Fixtures",
                column: "idAwayTeam");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_idHomeTeam",
                schema: "SN",
                table: "Fixtures",
                column: "idHomeTeam");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Teams_idAwayTeam",
                schema: "SN",
                table: "Fixtures",
                column: "idAwayTeam",
                principalSchema: "SN",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Teams_idHomeTeam",
                schema: "SN",
                table: "Fixtures",
                column: "idHomeTeam",
                principalSchema: "SN",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Teams_idAwayTeam",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Teams_idHomeTeam",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_idAwayTeam",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_idHomeTeam",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamID",
                schema: "SN",
                table: "Fixtures",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamID",
                schema: "SN",
                table: "Fixtures",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_AwayTeamID",
                schema: "SN",
                table: "Fixtures",
                column: "AwayTeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_HomeTeamID",
                schema: "SN",
                table: "Fixtures",
                column: "HomeTeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Teams_AwayTeamID",
                schema: "SN",
                table: "Fixtures",
                column: "AwayTeamID",
                principalSchema: "SN",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Teams_HomeTeamID",
                schema: "SN",
                table: "Fixtures",
                column: "HomeTeamID",
                principalSchema: "SN",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
