using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriPlan.Models
{
    [Table("objetivos")]
    public class Objetivo
    {
        [Key]
        public int IdObjetivo { get; set; }
        
        [Required]
        public string Nombre { get; set; }
        
        public string? Descripcion { get; set; }
    }
}