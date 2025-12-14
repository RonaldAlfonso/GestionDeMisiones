using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDeMisiones.Migrations
{
    /// <inheritdoc />
    public partial class TrasladoSupervisado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrasladoSupervisado",
                columns: table => new
                {
                    SupervisoresId = table.Column<int>(type: "int", nullable: false),
                    TrasladosSupervisadosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrasladoSupervisado", x => new { x.SupervisoresId, x.TrasladosSupervisadosId });
                    table.ForeignKey(
                        name: "FK_TrasladoSupervisado_PersonalDeApoyo_SupervisoresId",
                        column: x => x.SupervisoresId,
                        principalTable: "PersonalDeApoyo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrasladoSupervisado_Traslados_TrasladosSupervisadosId",
                        column: x => x.TrasladosSupervisadosId,
                        principalTable: "Traslados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrasladoSupervisado_TrasladosSupervisadosId",
                table: "TrasladoSupervisado",
                column: "TrasladosSupervisadosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrasladoSupervisado");
        }
    }
}
