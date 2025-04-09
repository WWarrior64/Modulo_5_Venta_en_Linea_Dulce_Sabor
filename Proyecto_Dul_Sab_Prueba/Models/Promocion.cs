using System.ComponentModel.DataAnnotations;

namespace Proyecto_Dul_Sab_Prueba.Models
{
    public class Promocion
    {

        [Key]
        [Display(Name = "Id")]
        public int promocionId { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Display(Name = "Imagen")]
        public string imgUrl { get; set; }

        [Display(Name = "Tipo (Descuento o exclusivo)")]
        public string tipo { get; set; }

        [Display(Name = "Precio")]
        public decimal precio { get; set; }

        [Display(Name = "Descuento")]
        public decimal descuento { get; set; }

        [Display(Name = "Fecha de inicio")]
        public DateTime fechainicio { get; set; }

        [Display(Name = "Fecha de finalización")]
        public DateTime fechafin { get; set; }
    }
}
