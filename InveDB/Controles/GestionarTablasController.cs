using InveDB.Datos;
using InveDB.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

namespace InveDB.Controles
{
    public class GestionarTablasController : Controller
    {
        private readonly ContextoAlmacen _context;
        private readonly EjecutarCmdWeb _cmd;

        public GestionarTablasController(ContextoAlmacen context, EjecutarCmdWeb cmd)
        {
            _context = context;
            _cmd = cmd;
        }

        // ===================== INDEX =====================
        public IActionResult Index()
        {
            try
            {
                var categorias = _context.Categorias.ToList();
                var productos = _context.Productos
                .Include(p => p.Categoria)
                 .ToList();
                var proveedores = _context.Proveedores.ToList();
                var sucursales = _context.Sucursales.ToList();

                ViewBag.Categorias = categorias;
                ViewBag.Productos = productos;
                ViewBag.Proveedores = proveedores;
                ViewBag.Sucursales = sucursales;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar los datos: " + ex.Message;
                return View();
            }
        }

        // ===================== CATEGORÍAS =====================
        [HttpPost]
        public IActionResult AgregarCategoria(string nombre, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return BadRequest("El nombre es obligatorio.");
            string sql = $"INSERT INTO Categoria (nombre, descripcion) VALUES ('{nombre}', '{descripcion ?? ""}')";
            _cmd.EjecutarNonQuery(sql);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditarCategoria(int idCategoria, string nombre, string descripcion)
        {
            string sql = $"UPDATE Categoria SET nombre='{nombre}', descripcion='{descripcion ?? ""}' WHERE id_categoria={idCategoria}";
            int filas = _cmd.EjecutarNonQuery(sql);

            if (filas == 0)
            {
                return BadRequest($"No se encontró la categoría con ID {idCategoria}.");
            }

            return Ok(); 
        }


        [HttpPost]
        public IActionResult EliminarCategoria(int idCategoria)
        {
            try
            {
                string sql = $"DELETE FROM Categoria WHERE id_categoria={idCategoria}";
                _cmd.EjecutarNonQuery(sql);
                return Ok();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                    return BadRequest("No se puede eliminar la categoría porque está asociada a productos.");
                return BadRequest("Error SQL: " + ex.Message);
            }
        }

        // ===================== PRODUCTOS =====================
        [HttpPost]
        public IActionResult AgregarProducto(string nombre, string descripcion, string unidadMedida, int idCategoria, decimal precioUnitario)
        {
            string sql = $@"
                INSERT INTO Producto (nombre, descripcion, unidad_medida, id_categoria, precio_unitario)
                VALUES ('{nombre}', '{descripcion ?? ""}', '{unidadMedida}', {idCategoria}, {precioUnitario})";
            _cmd.EjecutarNonQuery(sql);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditarProducto(int idProducto, string nombre, string descripcion, string unidadMedida, int idCategoria, decimal precioUnitario)
        {
            string sql = $@"
                UPDATE Producto 
                SET nombre='{nombre}', descripcion='{descripcion ?? ""}', unidad_medida='{unidadMedida}', 
                    id_categoria={idCategoria}, precio_unitario={precioUnitario}
                WHERE id_producto={idProducto}";
            _cmd.EjecutarNonQuery(sql);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarProducto(int idProducto)
        {
            try
            {
                string sql = $"DELETE FROM Producto WHERE id_producto={idProducto}";
                _cmd.EjecutarNonQuery(sql);
                return Ok();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                    return BadRequest("No se puede eliminar el producto porque está relacionado con inventarios o movimientos.");
                return BadRequest("Error SQL: " + ex.Message);
            }
        }

        // ===================== PROVEEDORES =====================
        [HttpPost]
        public IActionResult AgregarProveedor(string nombre, string telefono, string direccion)
        {
            string sql = $@"
                INSERT INTO Proveedor (nombre, telefono, direccion)
                VALUES ('{nombre}', '{telefono}', '{direccion ?? ""}')";
            _cmd.EjecutarNonQuery(sql);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditarProveedor(int idProveedor, string nombre, string telefono, string direccion)
        {
            string sql = $@"
                UPDATE Proveedor 
                SET nombre='{nombre}', telefono='{telefono}', direccion='{direccion ?? ""}' 
                WHERE id_proveedor={idProveedor}";
            _cmd.EjecutarNonQuery(sql);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarProveedor(int idProveedor)
        {
            try
            {
                string sql = $"DELETE FROM Proveedor WHERE id_proveedor={idProveedor}";
                _cmd.EjecutarNonQuery(sql);
                return Ok();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                    return BadRequest("No se puede eliminar el proveedor porque tiene registros asociados.");
                return BadRequest("Error SQL: " + ex.Message);
            }
        }

        // ===================== SUCURSALES =====================
        [HttpPost]
        public IActionResult AgregarSucursal(string nombre, string direccion, string telefono)
        {
            string sql = $@"
                INSERT INTO Sucursal (nombre, direccion, telefono)
                VALUES ('{nombre}', '{direccion ?? ""}', '{telefono}')";
            _cmd.EjecutarNonQuery(sql);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditarSucursal(int idSucursal, string nombre, string direccion, string telefono)
        {
            string sql = $@"
                UPDATE Sucursal 
                SET nombre='{nombre}', direccion='{direccion ?? ""}', telefono='{telefono}' 
                WHERE id_sucursal={idSucursal}";
            _cmd.EjecutarNonQuery(sql);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarSucursal(int idSucursal)
        {
            try
            {
                string sql = $"DELETE FROM Sucursal WHERE id_sucursal={idSucursal}";
                _cmd.EjecutarNonQuery(sql);
                return Ok();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                    return BadRequest("No se puede eliminar la sucursal porque tiene registros asociados.");
                return BadRequest("Error SQL: " + ex.Message);
            }
        }
    }

}
