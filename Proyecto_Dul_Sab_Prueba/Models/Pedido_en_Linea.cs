using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Dul_Sab_Prueba.Models
{
    public class Pedido_en_Linea
    {
        [Key]
        [Display(Name = "ID del Pedido")]
        public int pedidoId { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int clienteId { get; set; }

        [Display(Name = "Fecha del Pedido")]
        public DateTime fechapedido { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Estado")]
        [RegularExpression("Pendiente|En preparacion|En camino|Entregado", ErrorMessage = "Estado inválido")]
        public string estado { get; set; }

        [Required]
        [Display(Name = "Método de Pago")]
        public int metodopagoId { get; set; }

        [Required]
        [Display(Name = "Total")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal total { get; set; } = 0;

        [Display(Name = "Facturado")]
        public bool facturado { get; set; } = false;

        [Display(Name = "Fecha de Factura")]
        public DateTime? fechafactura { get; set; }

        public Clientes cliente { get; set; }

        public ICollection<Detalle_Pedido_en_Linea> detalles { get; set; }
    }
}
