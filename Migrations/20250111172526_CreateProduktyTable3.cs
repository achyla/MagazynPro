using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazynPro.Migrations
{
    /// <inheritdoc />
    public partial class CreateProduktyTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nazwa",
                table: "Produkty",
                newName: "NazwaProduktu");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Klienci",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Klienci");

            migrationBuilder.RenameColumn(
                name: "NazwaProduktu",
                table: "Produkty",
                newName: "Nazwa");
        }
    }
}
