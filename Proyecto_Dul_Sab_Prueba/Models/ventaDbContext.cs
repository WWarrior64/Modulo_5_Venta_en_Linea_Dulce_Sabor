using Microsoft.EntityFrameworkCore;

namespace Proyecto_Dul_Sab_Prueba.Models
{
    public class ventaDbContext : DbContext
    {

        public ventaDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Platillo> Platillo { get; set; }
        public DbSet<Combo> Combo { get; set; }
        public DbSet<Promocion> Promocion { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<SubCategoria> SubCategoria { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Pedido_en_Linea> Pedido_En_Linea { get; set; }
        public DbSet<Detalle_Pedido_en_Linea> Detalle_Pedido_En_Linea { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Metodo_Pago> Metodo_Pago { get; set; }
        public DbSet<Ubicacion_Geografica> Ubicacion_Geografica { get; set; }

    }
}
