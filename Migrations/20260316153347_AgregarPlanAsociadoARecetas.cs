using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriPlan.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPlanAsociadoARecetas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlanAsociado",
                table: "recetas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanAsociado",
                table: "recetas");
        }
    }
}
