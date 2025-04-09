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

    }
}
