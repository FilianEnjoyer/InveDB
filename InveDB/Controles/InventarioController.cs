using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using InveDB.Modelos;

namespace InveDB.Controles
{
    public class InventarioController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;

        public InventarioController(IConfiguration configuration, IHttpContextAccessor context)
        {
            _configuration = configuration;
            _context = context;
        }

        public IActionResult Index(DateTime? fecha, string? proveedor, string? categoria, string? producto)

        {

            List<InventarioViewModel> lista = new();
            string connectionString =
              _context.HttpContext.Session.GetString("ConexionActiva")
              ?? _configuration.GetConnectionString("DefaultConnection");


            using (SqlConnection con = new(connectionString))
            {
                using (SqlCommand cmd = new("sp_ObtenerInventarioFiltrado", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fecha", (object?)fecha ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@nombreProveedor", (object?)proveedor ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@nombreCategoria", (object?)categoria ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@busquedaProducto", (object?)producto ?? DBNull.Value);


                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new InventarioViewModel
                            {
                                IdProducto = (int)dr["id_producto"],
                                Producto = dr["Producto"].ToString(),
                                Categoria = dr["Categoria"].ToString(),

                                Cantidad = Convert.ToDecimal(dr["Cantidad_Actual"]),
                                UnidadMedida = dr["unidad_medida"].ToString(),
                                PrecioUnitario = Convert.ToDecimal(dr["Precio_Unitario"]),
                                Descripcion = dr["descripcion"].ToString(),


                                Proveedor = dr["Proveedor"] == DBNull.Value ? "—" : dr["Proveedor"].ToString(),
                                UltimaFecha = dr["Ultima_Fecha"] == DBNull.Value ? null : Convert.ToDateTime(dr["Ultima_Fecha"])
                            });
                        }
                    }
                }
            }

            // Poblar los filtros (comboboxes)
            ViewBag.Proveedores = ObtenerLista("SELECT DISTINCT nombre FROM Proveedor ORDER BY nombre", connectionString);

            ViewBag.Categorias = ObtenerLista("SELECT DISTINCT nombre FROM Categoria ORDER BY nombre", connectionString);
            ViewBag.Productos = ObtenerLista("SELECT DISTINCT nombre FROM Producto ORDER BY nombre", connectionString);
            ViewBag.Fechas = ObtenerLista("SELECT DISTINCT CAST(fecha AS DATE) AS fecha FROM Movimiento ORDER BY fecha DESC", connectionString);




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


                        while (dr.Read())
                        {
                            lista.Add(dr[0].ToString());
                        }
                    }
                }
            }



            return lista;
        }
    }
}
