using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriPlan.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPlanElegido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlanElegido",
                table: "usuarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanElegido",
                table: "usuarios");
        }
    }
}
