using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class Adicicionaasmatchesnocontexto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicMatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    PlayMode = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Board = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicMatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicMatchPlayers",
                columns: table => new
                {
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicMatchPlayers", x => new { x.MatchId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_TicMatchPlayers_TicMatches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "TicMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicMatchPlayers_TicPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "TicPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WinningSymbol = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    WinningPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Tie = table.Column<bool>(type: "bit", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicScores_TicMatches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "TicMatches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicScores_TicPlayers_WinningPlayerId",
                        column: x => x.WinningPlayerId,
                        principalTable: "TicPlayers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicMatchPlayers_PlayerId",
                table: "TicMatchPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TicScores_MatchId",
                table: "TicScores",
                column: "MatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicScores_WinningPlayerId",
                table: "TicScores",
                column: "WinningPlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicMatchPlayers");

            migrationBuilder.DropTable(
                name: "TicScores");

            migrationBuilder.DropTable(
                name: "TicMatches");
        }
    }
}
