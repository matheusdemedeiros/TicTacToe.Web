using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class Adicionaoconceitodojogadoratual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentPlayerId",
                table: "TicMatches",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TicMatches_CurrentPlayerId",
                table: "TicMatches",
                column: "CurrentPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicMatches_TicPlayers_CurrentPlayerId",
                table: "TicMatches",
                column: "CurrentPlayerId",
                principalTable: "TicPlayers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicMatches_TicPlayers_CurrentPlayerId",
                table: "TicMatches");

            migrationBuilder.DropIndex(
                name: "IX_TicMatches_CurrentPlayerId",
                table: "TicMatches");

            migrationBuilder.DropColumn(
                name: "CurrentPlayerId",
                table: "TicMatches");
        }
    }
}
