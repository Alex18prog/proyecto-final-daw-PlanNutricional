namespace NutriPlan.Models.ViewModels
{
    public class MiPlanViewModel
    {
        public string DiaSemana { get; set; }  // "Lunes", "Martes", etc.
        public string MomentoDia { get; set; } // "Desayuno", "Comida", "Cena"
        public string NombreReceta { get; set; }
        public int Calorias { get; set; }
    }
}