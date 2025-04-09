using System.ComponentModel.DataAnnotations;

namespace Proyecto_Dul_Sab_Prueba.Models
{
    public class Metodo_Pago
    {
        [Key]
        [Display(Name = "ID")]
        public int metodopagoId { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [StringLength(50)]
        public string nombre { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(255)]
        public string? descripcion { get; set; }
    }
}
