using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Dul_Sab_Prueba.Models
{
    public class Detalle_Pedido_en_Linea
    {
        [Key]
        [Display(Name = "ID Detalle")]
        public int detallepedidoId { get; set; }

        [Required]
        [Display(Name = "ID Pedido")]
        public int pedidoId { get; set; }

        [Display(Name = "ID Ítem")]
        public int? itemId { get; set; }

        [Required]
        [Display(Name = "Tipo")]
        [RegularExpression("platillo|combo|promocion", ErrorMessage = "Tipo inválido")]
        public string tipo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Cantidad debe ser mayor que 0")]
        public int cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal preciounitario { get; set; }

        [NotMapped]
        public decimal subtotal => cantidad * preciounitario;

        public Pedido_en_Linea pedido { get; set; }

        public Item item { get; set; }
    }
}
