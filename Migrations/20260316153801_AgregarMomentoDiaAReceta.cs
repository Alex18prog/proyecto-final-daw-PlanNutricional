using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriPlan.Migrations
{
    /// <inheritdoc />
    public partial class AgregarMomentoDiaAReceta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MomentoDia",
                table: "recetas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MomentoDia",
                table: "recetas");
        }
    }
}
