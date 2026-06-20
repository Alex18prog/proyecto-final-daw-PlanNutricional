using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriPlan.Models
{
    [Table("lista_compra")]
    public class ListaCompra
    {
        [Key]
        public int IdLista { get; set; }
        public int IdUsuario { get; set; }
        public string Producto { get; set; }
        public bool Comprado { get; set; }
    }
}