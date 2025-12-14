using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDeMisiones.Migrations
{
    /// <inheritdoc />
    public partial class SubordinacionMigr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subordinaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaestroId = table.Column<int>(type: "int", nullable: false),
                    DiscipuloId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TipoRelacion = table.Column<int>(type: "int", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subordinaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subordinaciones_Hechiceros_DiscipuloId",
                        column: x => x.DiscipuloId,
                        principalTable: "Hechiceros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subordinaciones_Hechiceros_MaestroId",
                        column: x => x.MaestroId,
                        principalTable: "Hechiceros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subordinaciones_DiscipuloId",
                table: "Subordinaciones",
                column: "DiscipuloId");

            migrationBuilder.CreateIndex(
                name: "IX_Subordinaciones_MaestroId_DiscipuloId_Activa",
                table: "Subordinaciones",
                columns: new[] { "MaestroId", "DiscipuloId", "Activa" },
                unique: true,
                filter: "[Activa] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subordinaciones");
        }
    }
}
