using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazynPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateZamowieniaModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zamowienia_Produkty_ProduktId",
                table: "Zamowienia");

            migrationBuilder.AlterColumn<int>(
                name: "ProduktId",
                table: "Zamowienia",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ilosc",
                table: "Produkty",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Zamowienia_Produkty_ProduktId",
                table: "Zamowienia",
                column: "ProduktId",
                principalTable: "Produkty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zamowienia_Produkty_ProduktId",
                table: "Zamowienia");

            migrationBuilder.DropColumn(
                name: "Ilosc",
                table: "Produkty");

            migrationBuilder.AlterColumn<int>(
                name: "ProduktId",
                table: "Zamowienia",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Zamowienia_Produkty_ProduktId",
                table: "Zamowienia",
                column: "ProduktId",
                principalTable: "Produkty",
                principalColumn: "Id");
        }
    }
}
