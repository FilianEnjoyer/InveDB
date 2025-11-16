using InveDB.Modelos;
using InveDB.Models;
using Microsoft.EntityFrameworkCore;
namespace InveDB.Datos
{
    public class ContextoAlmacen : Microsoft.EntityFrameworkCore.DbContext
    {
        public ContextoAlmacen(DbContextOptions<ContextoAlmacen> options) : base(options) { }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }


    }
}
