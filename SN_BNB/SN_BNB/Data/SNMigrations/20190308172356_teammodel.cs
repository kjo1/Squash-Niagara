using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SN_BNB.Data.SNMigrations
{
    public partial class teammodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Locations_LocationID",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Seasons_SeasonID",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropTable(
                name: "Approved",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Fixture_has_Team",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Locations",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "MatchScores",
                schema: "SN");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_LocationID",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_SeasonID",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "AwayTeamID",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.RenameColumn(
                name: "Datetime",
                schema: "SN",
                table: "Matches",
                newName: "MatchDateTime");

            migrationBuilder.RenameColumn(
                name: "SeasonID",
                schema: "SN",
                table: "Fixtures",
                newName: "idHomeTeam");

            migrationBuilder.RenameColumn(
                name: "LocationID",
                schema: "SN",
                table: "Fixtures",
                newName: "idAwayTeam");

            migrationBuilder.RenameColumn(
                name: "HomeTeamID",
                schema: "SN",
                table: "Fixtures",
                newName: "Season_idSeason");

            migrationBuilder.RenameColumn(
                name: "Datetime",
                schema: "SN",
                table: "Fixtures",
                newName: "FixtureDateTime");

            migrationBuilder.AlterColumn<string>(
                name: "FixtureScore",
                schema: "SN",
                table: "TeamScores",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "TeamScoreApprovedBy",
                schema: "SN",
                table: "TeamScores",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                schema: "SN",
                table: "Players",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "SN",
                table: "Players",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "Loss",
                schema: "SN",
                table: "Players",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Win",
                schema: "SN",
                table: "Players",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatchPosition",
                schema: "SN",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Player1ID",
                schema: "SN",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Player1Score",
                schema: "SN",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Player2ID",
                schema: "SN",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Player2Score",
                schema: "SN",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamScoreID",
                schema: "SN",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FixtureLocationAddress",
                schema: "SN",
                table: "Fixtures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FixtureLocationCity",
                schema: "SN",
                table: "Fixtures",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "News",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player1ID",
                schema: "SN",
                table: "Matches",
                column: "Player1ID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player2ID",
                schema: "SN",
                table: "Matches",
                column: "Player2ID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TeamScoreID",
                schema: "SN",
                table: "Matches",
                column: "TeamScoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_Season_idSeason",
                schema: "SN",
                table: "Fixtures",
                column: "Season_idSeason");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Seasons_Season_idSeason",
                schema: "SN",
                table: "Fixtures",
                column: "Season_idSeason",
                principalSchema: "SN",
                principalTable: "Seasons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_Player1ID",
                schema: "SN",
                table: "Matches",
                column: "Player1ID",
                principalSchema: "SN",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_Player2ID",
                schema: "SN",
                table: "Matches",
                column: "Player2ID",
                principalSchema: "SN",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_TeamScores_TeamScoreID",
                schema: "SN",
                table: "Matches",
                column: "TeamScoreID",
                principalSchema: "SN",
                principalTable: "TeamScores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Seasons_Season_idSeason",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_Player1ID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_Player2ID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_TeamScores_TeamScoreID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "News",
                schema: "SN");

            migrationBuilder.DropIndex(
                name: "IX_Matches_Player1ID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_Player2ID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_TeamScoreID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_Season_idSeason",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "TeamScoreApprovedBy",
                schema: "SN",
                table: "TeamScores");

            migrationBuilder.DropColumn(
                name: "TeamLosses",
                schema: "SN",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamWins",
                schema: "SN",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Loss",
                schema: "SN",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Win",
                schema: "SN",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "MatchPosition",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Player1ID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Player1Score",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Player2ID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Player2Score",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TeamScoreID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "FixtureLocationAddress",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "FixtureLocationCity",
                schema: "SN",
                table: "Fixtures");

            migrationBuilder.RenameColumn(
                name: "MatchDateTime",
                schema: "SN",
                table: "Matches",
                newName: "Datetime");

            migrationBuilder.RenameColumn(
                name: "idHomeTeam",
                schema: "SN",
                table: "Fixtures",
                newName: "SeasonID");

            migrationBuilder.RenameColumn(
                name: "idAwayTeam",
                schema: "SN",
                table: "Fixtures",
                newName: "LocationID");

            migrationBuilder.RenameColumn(
                name: "Season_idSeason",
                schema: "SN",
                table: "Fixtures",
                newName: "HomeTeamID");

            migrationBuilder.RenameColumn(
                name: "FixtureDateTime",
                schema: "SN",
                table: "Fixtures",
                newName: "Datetime");

            migrationBuilder.AlterColumn<string>(
                name: "FixtureScore",
                schema: "SN",
                table: "TeamScores",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                schema: "SN",
                table: "Players",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "SN",
                table: "Players",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 45);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamID",
                schema: "SN",
                table: "Fixtures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Approved",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamScoreID = table.Column<int>(nullable: true),
                    TeamScoresID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approved", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Approved_TeamScores_TeamScoreID",
                        column: x => x.TeamScoreID,
                        principalSchema: "SN",
                        principalTable: "TeamScores",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Approved_Users_UserID",
                        column: x => x.UserID,
                        principalSchema: "SN",
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fixture_has_Team",
                schema: "SN",
                columns: table => new
                {
                    TeamID = table.Column<int>(nullable: false),
                    FixtureID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixture_has_Team", x => new { x.TeamID, x.FixtureID });
                    table.ForeignKey(
                        name: "FK_Fixture_has_Team_Fixtures_FixtureID",
                        column: x => x.FixtureID,
                        principalSchema: "SN",
                        principalTable: "Fixtures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fixture_has_Team_Teams_TeamID",
                        column: x => x.TeamID,
                        principalSchema: "SN",
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MatchScores",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MatchScoresPoints = table.Column<int>(nullable: false),
                    PlayerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchScores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MatchScores_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalSchema: "SN",
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_LocationID",
                schema: "SN",
                table: "Fixtures",
                column: "LocationID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_SeasonID",
                schema: "SN",
                table: "Fixtures",
                column: "SeasonID");

            migrationBuilder.CreateIndex(
                name: "IX_Approved_TeamScoreID",
                schema: "SN",
                table: "Approved",
                column: "TeamScoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Approved_UserID",
                schema: "SN",
                table: "Approved",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_has_Team_FixtureID",
                schema: "SN",
                table: "Fixture_has_Team",
                column: "FixtureID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchScores_PlayerID",
                schema: "SN",
                table: "MatchScores",
                column: "PlayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Locations_LocationID",
                schema: "SN",
                table: "Fixtures",
                column: "LocationID",
                principalSchema: "SN",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Seasons_SeasonID",
                schema: "SN",
                table: "Fixtures",
                column: "SeasonID",
                principalSchema: "SN",
                principalTable: "Seasons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
