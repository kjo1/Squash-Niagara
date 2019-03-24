using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SN_BNB.Data.SNMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SN");

            migrationBuilder.CreateTable(
                name: "Divisions",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DivisionName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SeasonStart = table.Column<DateTime>(nullable: false),
                    SeasonEnd = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserRole = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamName = table.Column<string>(nullable: false),
                    TeamPoints = table.Column<int>(nullable: false),
                    TeamCreatedOn = table.Column<DateTime>(nullable: true),
                    DivisionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Teams_Divisions_DivisionID",
                        column: x => x.DivisionID,
                        principalSchema: "SN",
                        principalTable: "Divisions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fixtures",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Datetime = table.Column<DateTime>(nullable: false),
                    HomeScore = table.Column<int>(nullable: false),
                    AwayScore = table.Column<int>(nullable: false),
                    HomeTeamID = table.Column<int>(nullable: false),
                    AwayTeamID = table.Column<int>(nullable: false),
                    SeasonID = table.Column<int>(nullable: false),
                    LocationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixtures", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Fixtures_Locations_LocationID",
                        column: x => x.LocationID,
                        principalSchema: "SN",
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fixtures_Seasons_SeasonID",
                        column: x => x.SeasonID,
                        principalSchema: "SN",
                        principalTable: "Seasons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    MiddleName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<int>(nullable: false),
                    Position = table.Column<string>(nullable: false),
                    TeamID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamID",
                        column: x => x.TeamID,
                        principalSchema: "SN",
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FixtureTeams",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FixtureID = table.Column<int>(nullable: false),
                    TeamID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixtureTeams", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FixtureTeams_Fixtures_FixtureID",
                        column: x => x.FixtureID,
                        principalSchema: "SN",
                        principalTable: "Fixtures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FixtureTeams_Teams_TeamID",
                        column: x => x.TeamID,
                        principalSchema: "SN",
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Datetime = table.Column<DateTime>(nullable: false),
                    FixtureID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Matches_Fixtures_FixtureID",
                        column: x => x.FixtureID,
                        principalSchema: "SN",
                        principalTable: "Fixtures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamScores",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FixtureScore = table.Column<string>(nullable: false),
                    TeamID = table.Column<int>(nullable: false),
                    FixtureID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamScores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TeamScores_Fixtures_FixtureID",
                        column: x => x.FixtureID,
                        principalSchema: "SN",
                        principalTable: "Fixtures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamScores_Teams_TeamID",
                        column: x => x.TeamID,
                        principalSchema: "SN",
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Approved",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamScoresID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    TeamScoreID = table.Column<int>(nullable: true)
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
                name: "IX_Divisions_DivisionName",
                schema: "SN",
                table: "Divisions",
                column: "DivisionName",
                unique: true);

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
                name: "IX_FixtureTeams_FixtureID",
                schema: "SN",
                table: "FixtureTeams",
                column: "FixtureID");

            migrationBuilder.CreateIndex(
                name: "IX_FixtureTeams_TeamID",
                schema: "SN",
                table: "FixtureTeams",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_FixtureID",
                schema: "SN",
                table: "Matches",
                column: "FixtureID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchScores_PlayerID",
                schema: "SN",
                table: "MatchScores",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamID",
                schema: "SN",
                table: "Players",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_DivisionID",
                schema: "SN",
                table: "Teams",
                column: "DivisionID");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamName",
                schema: "SN",
                table: "Teams",
                column: "TeamName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamScores_FixtureID",
                schema: "SN",
                table: "TeamScores",
                column: "FixtureID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamScores_TeamID",
                schema: "SN",
                table: "TeamScores",
                column: "TeamID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Approved",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "FixtureTeams",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Matches",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "MatchScores",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "TeamScores",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Players",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Fixtures",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Teams",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Locations",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Seasons",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Divisions",
                schema: "SN");
        }
    }
}
