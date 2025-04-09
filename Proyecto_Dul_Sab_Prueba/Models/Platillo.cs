using System.ComponentModel.DataAnnotations;

namespace Proyecto_Dul_Sab_Prueba.Models
{
    public class Platillo
    {
        [Key]
        [Display(Name = "Id")]
        public int platilloId { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Display(Name = "Imagen")]
        public string imgUrl { get; set; }

        [Display(Name = "Categoria")]
        public int categoriaId { get; set; }

        [Display(Name = "Subcategoria")]
        public int? subcategoriaId { get; set; }

        [Display(Name = "Precio")]
        public decimal precio { get; set; }

        [Display(Name = "Disponibilidad")]
        public bool disponible { get; set; }
    }
}
