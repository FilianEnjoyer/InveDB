using System.Collections.Generic;
using InveDB.Modelos;
namespace InveDB.Modelos
{
    public class GestionarTablasViewModel
    {
        public List<Producto> Productos { get; set; } = new();
        public List<Proveedor> Proveedores { get; set; } = new();
        public List<Categoria> Categorias { get; set; } = new();
        public List<Sucursal> Sucursales { get; set; } = new();
        public List<Inventario> Inventarios { get; set; } = new();
        public List<Movimiento> Movimientos { get; set; } = new();
    }
}
