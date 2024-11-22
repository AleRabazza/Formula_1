using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Formula_1.Migrations
{
    /// <inheritdoc />
    public partial class Reinicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carreras",
                columns: table => new
                {
                    IdCarrera = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fecha = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreras", x => x.IdCarrera);
                });

            migrationBuilder.CreateTable(
                name: "Escuderia",
                columns: table => new
                {
                    IdEscuderia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PuntajeAcumulado = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaisDeOrigen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SponsorPrincipal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CantidadDePilotos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escuderia", x => x.IdEscuderia);
                });

            migrationBuilder.CreateTable(
                name: "Resultados",
                columns: table => new
                {
                    IdResultado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCarrera = table.Column<int>(type: "int", nullable: false),
                    IdPiloto = table.Column<int>(type: "int", nullable: false),
                    PosicionSalida = table.Column<int>(type: "int", nullable: false),
                    PosicionLlegada = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resultados", x => x.IdResultado);
                });

            migrationBuilder.CreateTable(
                name: "Pilotos",
                columns: table => new
                {
                    NumeroPiloto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroAuto = table.Column<int>(type: "int", nullable: false),
                    NombrePiloto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNac = table.Column<DateOnly>(type: "date", nullable: false),
                    PaisDeOrigen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PuntajeAcumulado = table.Column<int>(type: "int", nullable: false),
                    EscuderiaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pilotos", x => x.NumeroPiloto);
                    table.ForeignKey(
                        name: "FK_Pilotos_Escuderia_EscuderiaId",
                        column: x => x.EscuderiaId,
                        principalTable: "Escuderia",
                        principalColumn: "IdEscuderia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pilotos_EscuderiaId",
                table: "Pilotos",
                column: "EscuderiaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carreras");

            migrationBuilder.DropTable(
                name: "Pilotos");

            migrationBuilder.DropTable(
                name: "Resultados");

            migrationBuilder.DropTable(
                name: "Escuderia");
        }
    }
}
