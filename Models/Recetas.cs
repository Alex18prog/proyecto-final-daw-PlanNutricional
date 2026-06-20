using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriPlan.Models
{
    [Table("recetas")]
    public class Receta
    {
        [Key]
        public int IdReceta { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Calorias { get; set; }
        
        // Nueva propiedad para la lista de la compra
        public string Ingredientes { get; set; } = "Pollo, Arroz, Verduras"; 
        public string PlanAsociado { get; set; }
        public string MomentoDia { get; set; } = string.Empty; // "Desayuno", "Comida", "Cena"
    }
}