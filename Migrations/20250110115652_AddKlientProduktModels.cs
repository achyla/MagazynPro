using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazynPro.Migrations
{
    /// <inheritdoc />
    public partial class AddKlientProduktModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KlientId",
                table: "Zamowienia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProduktId",
                table: "Zamowienia",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Klienci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klienci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produkty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cena = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produkty", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienia_KlientId",
                table: "Zamowienia",
                column: "KlientId");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienia_ProduktId",
                table: "Zamowienia",
                column: "ProduktId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zamowienia_Klienci_KlientId",
                table: "Zamowienia",
                column: "KlientId",
                principalTable: "Klienci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zamowienia_Produkty_ProduktId",
                table: "Zamowienia",
                column: "ProduktId",
                principalTable: "Produkty",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zamowienia_Klienci_KlientId",
                table: "Zamowienia");

            migrationBuilder.DropForeignKey(
                name: "FK_Zamowienia_Produkty_ProduktId",
                table: "Zamowienia");

            migrationBuilder.DropTable(
                name: "Klienci");

            migrationBuilder.DropTable(
                name: "Produkty");

            migrationBuilder.DropIndex(
                name: "IX_Zamowienia_KlientId",
                table: "Zamowienia");

            migrationBuilder.DropIndex(
                name: "IX_Zamowienia_ProduktId",
                table: "Zamowienia");

            migrationBuilder.DropColumn(
                name: "KlientId",
                table: "Zamowienia");

            migrationBuilder.DropColumn(
                name: "ProduktId",
                table: "Zamowienia");
        }
    }
}
