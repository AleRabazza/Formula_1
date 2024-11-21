using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Formula_1.Migrations
{
    /// <inheritdoc />
    public partial class sumarCampoAEscuderias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadDePilotos",
                table: "Escuderia",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadDePilotos",
                table: "Escuderia");
        }
    }
}
