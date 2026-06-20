using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriPlan.Models
{
    [Table("plan_semanal")]
    public class PlanSemanal 
    {
        [Key]
        public int IdPlanSemanal { get; set; }
        
        public int IdUsuario { get; set; } 
        public DateTime FechaInicio { get; set; }
        
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; } 
    }
}