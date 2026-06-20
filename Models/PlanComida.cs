using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriPlan.Models
{
    [Table("plan_comida")]
    public class PlanComida
    {
        [Key]
        public int IdPlanComida { get; set; }
        public string MomentoDia { get; set; } = string.Empty; // "Desayuno", "Comida", "Cena"
        public string DiaSemana { get; set; } = string.Empty;  // "Lunes", "Martes", etc.
        public int IdPlanSemanal { get; set; }
        public int IdReceta { get; set; }
    }
}