using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddShortCodeToTicMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortCode",
                table: "TicMatches",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE TicMatches
                SET ShortCode = LEFT(REPLACE(NEWID(), '-', ''), 6)
                WHERE ShortCode = '';
            ");

            migrationBuilder.CreateIndex(
                name: "IX_TicMatches_ShortCode",
                table: "TicMatches",
                column: "ShortCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicMatches_ShortCode",
                table: "TicMatches");

            migrationBuilder.DropColumn(
                name: "ShortCode",
                table: "TicMatches");
        }
    }
}
