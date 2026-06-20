using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriPlan.Migrations
{
    /// <inheritdoc />
    public partial class AgregarIngredientesARecetas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "recetas");

            migrationBuilder.AddColumn<string>(
                name: "Ingredientes",
                table: "recetas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ingredientes",
                table: "recetas");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "recetas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
