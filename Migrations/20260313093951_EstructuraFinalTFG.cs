using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriPlan.Migrations
{
    /// <inheritdoc />
    public partial class EstructuraFinalTFG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lista_compra",
                columns: table => new
                {
                    IdLista = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Producto = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comprado = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lista_compra", x => x.IdLista);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "objetivos",
                columns: table => new
                {
                    IdObjetivo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_objetivos", x => x.IdObjetivo);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "plan_comida",
                columns: table => new
                {
                    IdPlanComida = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MomentoDia = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdPlanSemanal = table.Column<int>(type: "int", nullable: false),
                    IdReceta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plan_comida", x => x.IdPlanComida);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "plan_semanal",
                columns: table => new
                {
                    IdPlanSemanal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plan_semanal", x => x.IdPlanSemanal);
                    table.ForeignKey(
                        name: "FK_plan_semanal_usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_IdObjetivo",
                table: "usuarios",
                column: "IdObjetivo");

            migrationBuilder.CreateIndex(
                name: "IX_plan_semanal_IdUsuario",
                table: "plan_semanal",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_usuarios_objetivos_IdObjetivo",
                table: "usuarios",
                column: "IdObjetivo",
                principalTable: "objetivos",
                principalColumn: "IdObjetivo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usuarios_objetivos_IdObjetivo",
                table: "usuarios");

            migrationBuilder.DropTable(
                name: "lista_compra");

            migrationBuilder.DropTable(
                name: "objetivos");

            migrationBuilder.DropTable(
                name: "plan_comida");

            migrationBuilder.DropTable(
                name: "plan_semanal");

            migrationBuilder.DropIndex(
                name: "IX_usuarios_IdObjetivo",
                table: "usuarios");
        }
    }
}
