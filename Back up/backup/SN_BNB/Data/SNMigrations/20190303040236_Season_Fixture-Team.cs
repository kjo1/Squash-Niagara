using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SN_BNB.Data.SNMigrations
{
    public partial class Season_FixtureTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixtureTeams_Fixtures_FixtureID",
                schema: "SN",
                table: "FixtureTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_FixtureTeams_Teams_TeamID",
                schema: "SN",
                table: "FixtureTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixtureTeams",
                schema: "SN",
                table: "FixtureTeams");

            migrationBuilder.DropIndex(
                name: "IX_FixtureTeams_TeamID",
                schema: "SN",
                table: "FixtureTeams");

            migrationBuilder.DropColumn(
                name: "ID",
                schema: "SN",
                table: "FixtureTeams");

            migrationBuilder.RenameTable(
                name: "FixtureTeams",
                schema: "SN",
                newName: "Fixture_has_Team",
                newSchema: "SN");

            migrationBuilder.RenameIndex(
                name: "IX_FixtureTeams_FixtureID",
                schema: "SN",
                table: "Fixture_has_Team",
                newName: "IX_Fixture_has_Team_FixtureID");

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                schema: "SN",
                table: "Players",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fixture_has_Team",
                schema: "SN",
                table: "Fixture_has_Team",
                columns: new[] { "TeamID", "FixtureID" });

            migrationBuilder.CreateTable(
                name: "SeasonTeams",
                schema: "SN",
                columns: table => new
                {
                    TeamID = table.Column<int>(nullable: false),
                    SeasonID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonTeams", x => new { x.TeamID, x.SeasonID });
                    table.ForeignKey(
                        name: "FK_SeasonTeams_Seasons_SeasonID",
                        column: x => x.SeasonID,
                        principalSchema: "SN",
                        principalTable: "Seasons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeasonTeams_Teams_TeamID",
                        column: x => x.TeamID,
                        principalSchema: "SN",
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTeams_SeasonID",
                schema: "SN",
                table: "SeasonTeams",
                column: "SeasonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixture_has_Team_Fixtures_FixtureID",
                schema: "SN",
                table: "Fixture_has_Team",
                column: "FixtureID",
                principalSchema: "SN",
                principalTable: "Fixtures",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixture_has_Team_Teams_TeamID",
                schema: "SN",
                table: "Fixture_has_Team",
                column: "TeamID",
                principalSchema: "SN",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixture_has_Team_Fixtures_FixtureID",
                schema: "SN",
                table: "Fixture_has_Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixture_has_Team_Teams_TeamID",
                schema: "SN",
                table: "Fixture_has_Team");

            migrationBuilder.DropTable(
                name: "SeasonTeams",
                schema: "SN");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fixture_has_Team",
                schema: "SN",
                table: "Fixture_has_Team");

            migrationBuilder.RenameTable(
                name: "Fixture_has_Team",
                schema: "SN",
                newName: "FixtureTeams",
                newSchema: "SN");

            migrationBuilder.RenameIndex(
                name: "IX_Fixture_has_Team_FixtureID",
                schema: "SN",
                table: "FixtureTeams",
                newName: "IX_FixtureTeams_FixtureID");

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                schema: "SN",
                table: "Players",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ID",
                schema: "SN",
                table: "FixtureTeams",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixtureTeams",
                schema: "SN",
                table: "FixtureTeams",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_FixtureTeams_TeamID",
                schema: "SN",
                table: "FixtureTeams",
                column: "TeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_FixtureTeams_Fixtures_FixtureID",
                schema: "SN",
                table: "FixtureTeams",
                column: "FixtureID",
                principalSchema: "SN",
                principalTable: "Fixtures",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FixtureTeams_Teams_TeamID",
                schema: "SN",
                table: "FixtureTeams",
                column: "TeamID",
                principalSchema: "SN",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
