using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SN_BNB.Data.SNMigrations
{
    public partial class cascadedeletematchscore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_TeamScores_TeamScoreTeamID_TeamScoreFixtureID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamScores_Fixtures_FixtureID",
                schema: "SN",
                table: "TeamScores");

            migrationBuilder.DropIndex(
                name: "IX_Matches_TeamScoreTeamID_TeamScoreFixtureID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TeamScoreFixtureID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TeamScoreTeamID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "SN",
                table: "News",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "SN",
                table: "News",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                schema: "SN",
                table: "News",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_TeamScores_Fixtures_FixtureID",
                schema: "SN",
                table: "TeamScores",
                column: "FixtureID",
                principalSchema: "SN",
                principalTable: "Fixtures",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamScores_Fixtures_FixtureID",
                schema: "SN",
                table: "TeamScores");

            migrationBuilder.DropColumn(
                name: "Date",
                schema: "SN",
                table: "News");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "SN",
                table: "News",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "SN",
                table: "News",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "TeamScoreFixtureID",
                schema: "SN",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamScoreTeamID",
                schema: "SN",
                table: "Matches",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TeamScoreTeamID_TeamScoreFixtureID",
                schema: "SN",
                table: "Matches",
                columns: new[] { "TeamScoreTeamID", "TeamScoreFixtureID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_TeamScores_TeamScoreTeamID_TeamScoreFixtureID",
                schema: "SN",
                table: "Matches",
                columns: new[] { "TeamScoreTeamID", "TeamScoreFixtureID" },
                principalSchema: "SN",
                principalTable: "TeamScores",
                principalColumns: new[] { "TeamID", "FixtureID" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamScores_Fixtures_FixtureID",
                schema: "SN",
                table: "TeamScores",
                column: "FixtureID",
                principalSchema: "SN",
                principalTable: "Fixtures",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
