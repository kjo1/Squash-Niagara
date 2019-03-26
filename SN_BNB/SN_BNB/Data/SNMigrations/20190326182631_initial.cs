using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SN_BNB.Data.SNMigrations
{
    public partial class initial : Migration
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
                name: "Location",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LocationName = table.Column<string>(nullable: false),
                    LocationCity = table.Column<string>(nullable: false),
                    LocationStreet = table.Column<string>(nullable: false),
                    LocationBuildingNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "News",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Content = table.Column<string>(maxLength: 5000, nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Season_Title = table.Column<string>(nullable: false),
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
                    TeamBio = table.Column<string>(nullable: true),
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
                    Title = table.Column<string>(nullable: true),
                    FixtureDateTime = table.Column<DateTime>(nullable: false),
                    HomeScore = table.Column<int>(nullable: false),
                    AwayScore = table.Column<int>(nullable: false),
                    idHomeTeam = table.Column<int>(nullable: false),
                    idAwayTeam = table.Column<int>(nullable: false),
                    BonusPoint = table.Column<string>(nullable: true),
                    Season_idSeason = table.Column<int>(nullable: false),
                    location_locationId = table.Column<int>(nullable: false),
                    LocationID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixtures", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Fixtures_Location_LocationID",
                        column: x => x.LocationID,
                        principalSchema: "SN",
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixtures_Seasons_Season_idSeason",
                        column: x => x.Season_idSeason,
                        principalSchema: "SN",
                        principalTable: "Seasons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_idAwayTeam",
                        column: x => x.idAwayTeam,
                        principalSchema: "SN",
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_idHomeTeam",
                        column: x => x.idHomeTeam,
                        principalSchema: "SN",
                        principalTable: "Teams",
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
                    FirstName = table.Column<string>(maxLength: 45, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 45, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    OrderOfStrength = table.Column<decimal>(nullable: false),
                    Phone = table.Column<long>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    Played = table.Column<int>(nullable: false),
                    Win = table.Column<int>(nullable: false),
                    Loss = table.Column<int>(nullable: false),
                    For = table.Column<int>(nullable: false),
                    Against = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    TeamID = table.Column<int>(nullable: false),
                    PlayerWantsInfoHidden = table.Column<bool>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Matches",
                schema: "SN",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Player1Score = table.Column<int>(nullable: false),
                    Player2Score = table.Column<int>(nullable: false),
                    MatchPosition = table.Column<int>(nullable: false),
                    MatchTime = table.Column<TimeSpan>(nullable: false),
                    FixtureID = table.Column<int>(nullable: false),
                    AssignedMatchPlayerID = table.Column<int>(nullable: false),
                    PlayerID = table.Column<int>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Matches_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalSchema: "SN",
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignedMatchPlayer",
                schema: "SN",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    MatchID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedMatchPlayer", x => new { x.MatchID, x.PlayerID });
                    table.ForeignKey(
                        name: "FK_AssignedMatchPlayer_Matches_MatchID",
                        column: x => x.MatchID,
                        principalSchema: "SN",
                        principalTable: "Matches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignedMatchPlayer_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalSchema: "SN",
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedMatchPlayer_PlayerID",
                schema: "SN",
                table: "AssignedMatchPlayer",
                column: "PlayerID");

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
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_Season_idSeason",
                schema: "SN",
                table: "Fixtures",
                column: "Season_idSeason");

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

            migrationBuilder.CreateIndex(
                name: "IX_Matches_FixtureID",
                schema: "SN",
                table: "Matches",
                column: "FixtureID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_PlayerID",
                schema: "SN",
                table: "Matches",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Email",
                schema: "SN",
                table: "Players",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamID",
                schema: "SN",
                table: "Players",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTeams_SeasonID",
                schema: "SN",
                table: "SeasonTeams",
                column: "SeasonID");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedMatchPlayer",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "News",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "SeasonTeams",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Matches",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Fixtures",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Players",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Location",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Seasons",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Teams",
                schema: "SN");

            migrationBuilder.DropTable(
                name: "Divisions",
                schema: "SN");
        }
    }
}
