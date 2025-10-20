using Microsoft.EntityFrameworkCore;

namespace InveDB.Modelos
{
    public class AlmacenPTestContext : DbContext
    {
        public AlmacenPTestContext(DbContextOptions<AlmacenPTestContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define las claves y relaciones personalizadas si es necesario
            modelBuilder.Entity<Inventario>()
                .HasKey(i => i.IdProducto);

            modelBuilder.Entity<Inventario>()
                .HasOne(i => i.Producto)
                .WithOne(p => p.Inventarios.FirstOrDefault())
                .HasForeignKey<Inventario>(i => i.IdProducto)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
