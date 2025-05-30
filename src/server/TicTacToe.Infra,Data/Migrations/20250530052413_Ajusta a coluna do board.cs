using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class Ajustaacolunadoboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Board",
                table: "TicMatches",
                newName: "BoardJson");

            migrationBuilder.AddColumn<string>(
                name: "WinningSimbol",
                table: "TicMatches",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WinningSimbol",
                table: "TicMatches");

            migrationBuilder.RenameColumn(
                name: "BoardJson",
                table: "TicMatches",
                newName: "Board");
        }
    }
}
