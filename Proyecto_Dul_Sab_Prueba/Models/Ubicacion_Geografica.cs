using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Dul_Sab_Prueba.Models
{
    public class Ubicacion_Geografica
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Ubicación")]
        public int ubicacionId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Departamento")]
        public string departamento { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Ciudad")]
        public string ciudad { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Código Postal")]
        public string codigopostal { get; set; }

        [ForeignKey("Clientes")]
        [Display(Name = "ID Cliente")]
        public int clienteId { get; set; }

        public virtual Clientes Cliente { get; set; }

        public string ubicacion => $"{departamento} - {ciudad}";
    }
}
