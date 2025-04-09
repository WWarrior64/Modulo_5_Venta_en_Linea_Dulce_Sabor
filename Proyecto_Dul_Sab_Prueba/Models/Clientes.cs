using System.ComponentModel.DataAnnotations;

namespace Proyecto_Dul_Sab_Prueba.Models
{
    public class Clientes
    {
        [Key]
        [Display(Name = "ID Cliente")]
        public int clienteId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Teléfono")]
        public string telefono { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string correo { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Contraseña")]
        public string contraseña { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string direccion { get; set; }

        public ICollection<Pedido_en_Linea> pedidos { get; set; }
    }
}
