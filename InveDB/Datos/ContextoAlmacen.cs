using Microsoft.EntityFrameworkCore;
using InveDB.Modelos;
namespace InveDB.Datos
{
    public class ContextoAlmacen : DbContext
    {
        public ContextoAlmacen(DbContextOptions<ContextoAlmacen> options) : base(options) { }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
    }
}
