using Microsoft.EntityFrameworkCore.Migrations;

namespace SN_BNB.Data.SNMigrations
{
    public partial class NewmatchModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_PlayerID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "AssignedMatchPlayer",
                schema: "SN");

            migrationBuilder.DropIndex(
                name: "IX_Matches_PlayerID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "OrderOfStrength",
                schema: "SN",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "AssignedMatchPlayerID",
                schema: "SN",
                table: "Matches",
                newName: "Player2ID");

            migrationBuilder.AddColumn<int>(
                name: "Player1ID",
                schema: "SN",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_Player1ID",
                schema: "SN",
                table: "Matches",
                column: "Player1ID",
                principalSchema: "SN",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_Player2ID",
                schema: "SN",
                table: "Matches",
                column: "Player2ID",
                principalSchema: "SN",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_Player1ID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_Player2ID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_Player1ID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_Player2ID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Player1ID",
                schema: "SN",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "Player2ID",
                schema: "SN",
                table: "Matches",
                newName: "AssignedMatchPlayerID");

            migrationBuilder.AddColumn<decimal>(
                name: "OrderOfStrength",
                schema: "SN",
                table: "Players",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PlayerID",
                schema: "SN",
                table: "Matches",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssignedMatchPlayer",
                schema: "SN",
                columns: table => new
                {
                    MatchID = table.Column<int>(nullable: false),
                    PlayerID = table.Column<int>(nullable: false)
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
                name: "IX_Matches_PlayerID",
                schema: "SN",
                table: "Matches",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedMatchPlayer_PlayerID",
                schema: "SN",
                table: "AssignedMatchPlayer",
                column: "PlayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_PlayerID",
                schema: "SN",
                table: "Matches",
                column: "PlayerID",
                principalSchema: "SN",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
