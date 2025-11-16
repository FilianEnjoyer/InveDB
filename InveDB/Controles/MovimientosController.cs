using InveDB.Datos;
using InveDB.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InveDB.Controles
{
    public class MovimientosController : Controller
    {
        private readonly EjecutarCmdWeb _cmd;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        public MovimientosController(IConfiguration configuration, EjecutarCmdWeb cmd, IHttpContextAccessor context)
        {
            _configuration = configuration;
            _cmd = cmd;
            _context = context;
        }

        // Mostrar movimientos
        public IActionResult Index()
        {
            List<MovimientoViewModel> lista = new();
            string connectionString = _context.HttpContext.Session.GetString("ConexionActiva")
              ?? _configuration.GetConnectionString("DefaultConnection");
            //Conexion a la base de datos

            using (SqlConnection con = new(connectionString))
            {
                string query = @"
                    SELECT M.id_movimiento, M.fecha, M.tipo, M.cantidad, 
                           P.nombre AS Producto, PR.nombre AS Proveedor, S.nombre AS Sucursal
                    FROM Movimiento M
                    LEFT JOIN Producto P ON M.id_producto = P.id_producto
                    LEFT JOIN Proveedor PR ON M.id_proveedor = PR.id_proveedor
                    LEFT JOIN Sucursal S ON M.id_sucursal = S.id_sucursal
                    ORDER BY M.fecha DESC";

                using (SqlCommand cmd = new(query, con))
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new MovimientoViewModel
                            {
                                IdMovimiento = (int)dr["id_movimiento"],
                                Fecha = Convert.ToDateTime(dr["fecha"]),
                                Tipo = dr["tipo"].ToString(),
                                Cantidad = Convert.ToDecimal(dr["cantidad"]),
                                Producto = dr["Producto"].ToString(),
                                Proveedor = dr["Proveedor"] == DBNull.Value ? "—" : dr["Proveedor"].ToString(),
                                Sucursal = dr["Sucursal"] == DBNull.Value ? "—" : dr["Sucursal"].ToString()
                            });
                        }
                    }
                }
            }

            //  ViewBag con IDs y nombres
            ViewBag.Productos = ObtenerListaConId("SELECT id_producto AS Id, nombre AS Nombre FROM Producto ORDER BY nombre", connectionString);
            ViewBag.Proveedores = ObtenerListaConId("SELECT id_proveedor AS Id, nombre AS Nombre FROM Proveedor ORDER BY nombre", connectionString);
            ViewBag.Sucursales = ObtenerListaConId("SELECT id_sucursal AS Id, nombre AS Nombre FROM Sucursal ORDER BY nombre", connectionString);
            ViewBag.Fechas = ObtenerLista("SELECT CAST(fecha AS DATE) AS fecha FROM Movimiento GROUP BY CAST(fecha AS DATE)", connectionString);

            return View(lista);
        }

        private List<string> ObtenerLista(string query, string connectionString)
        {
            List<string> lista = new();
            using (SqlConnection con = new(connectionString))
            {
                using (SqlCommand cmd = new(query, con))
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read()) lista.Add(dr[0].ToString());
                    }
                }
            }
            return lista;
        }

        private List<(int Id, string Nombre)> ObtenerListaConId(string query, string connectionString)
        {
            List<(int, string)> lista = new();
            using (SqlConnection con = new(connectionString))
            {
                using (SqlCommand cmd = new(query, con))
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                            lista.Add((Convert.ToInt32(dr["Id"]), dr["Nombre"].ToString()));
                    }
                }
            }
            return lista;
        }

        //  Registrar movimiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegistrarMovimiento(int idProducto, string tipo, decimal cantidad, int? idProveedor, int? idSucursal)
        {
            try
            {
                //  Validar cantidad y tipo
                if (cantidad <= 0 || (tipo != "E" && tipo != "S"))
                    throw new Exception("Datos de movimiento inválidos.");

                //  Registrar el movimiento
                string sqlInsert = $@"
            INSERT INTO Movimiento (id_producto, tipo, cantidad, fecha, id_proveedor, id_sucursal)
            VALUES ({idProducto}, '{tipo}', {cantidad}, GETDATE(),
                    {(idProveedor.HasValue ? idProveedor.ToString() : "NULL")},
                    {(idSucursal.HasValue ? idSucursal.ToString() : "NULL")})";

                _cmd.EjecutarNonQuery(sqlInsert);

                

                //  Redirigir con TempData (evita doble envío en recarga)
                TempData["Mensaje"] = "Movimiento registrado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = " Error al registrar movimiento: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
        public decimal DetectarSiEsPar(decimal numero)
        {
            if (numero % 2 == 0)
            {
                return numero/2; 
            }
            else
            {
                return (numero/2)+2; 
            }

        }

    } 
    

    

    public class MovimientoViewModel
    {
        public int IdMovimiento { get; set; }
        public string Producto { get; set; }
        public string Tipo { get; set; }
        public decimal Cantidad { get; set; }
        public string Proveedor { get; set; }
        public string Sucursal { get; set; }
        public DateTime Fecha { get; set; }
    }
}
