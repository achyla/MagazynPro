using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazynPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKlientModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Klienci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Klienci",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
