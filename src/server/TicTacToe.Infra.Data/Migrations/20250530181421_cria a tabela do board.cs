using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class criaatabeladoboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoardJson",
                table: "TicMatches");

            migrationBuilder.DropColumn(
                name: "WinningSimbol",
                table: "TicMatches");

            migrationBuilder.AddColumn<Guid>(
                name: "TicBoardId",
                table: "TicMatches",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TicBoards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WinningSimbol = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Board = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicBoards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicMatches_TicBoardId",
                table: "TicMatches",
                column: "TicBoardId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TicMatches_TicBoards_TicBoardId",
                table: "TicMatches",
                column: "TicBoardId",
                principalTable: "TicBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicMatches_TicBoards_TicBoardId",
                table: "TicMatches");

            migrationBuilder.DropTable(
                name: "TicBoards");

            migrationBuilder.DropIndex(
                name: "IX_TicMatches_TicBoardId",
                table: "TicMatches");

            migrationBuilder.DropColumn(
                name: "TicBoardId",
                table: "TicMatches");

            migrationBuilder.AddColumn<string>(
                name: "BoardJson",
                table: "TicMatches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WinningSimbol",
                table: "TicMatches",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);
        }
    }
}
