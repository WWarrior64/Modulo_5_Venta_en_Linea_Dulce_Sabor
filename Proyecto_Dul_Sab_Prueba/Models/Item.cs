namespace Proyecto_Dul_Sab_Prueba.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public int? platilloId { get; set; }
        public int? comboId { get; set; }
        public int? promocionId { get; set; }
        public int cantidad { get; set; }
        public string? estado {  get; set; }
    }
}
