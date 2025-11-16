using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using InveDB.Models;

namespace InveDB.Controles
{
    public class InicioController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;

        public InicioController(IConfiguration configuration, IHttpContextAccessor context)
        {
            _configuration = configuration;
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new InicioViewModel();

            string conn = _context.HttpContext?.Session.GetString("ConexionActiva")
                          ?? _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();

                    // Contadores (si alguna tabla no existe, capturamos la excepción y dejamos 0)
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Usuarios", con))
                        model.CountUsuarios = Convert.ToInt32(cmd.ExecuteScalar());

                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Producto", con))
                        model.CountProductos = Convert.ToInt32(cmd.ExecuteScalar());

                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Movimiento", con))
                        model.CountMovimientos = Convert.ToInt32(cmd.ExecuteScalar());

                    // Últimos 3 movimientos
                    string sql = @"
                        SELECT TOP 3 m.id_movimiento, ISNULL(p.nombre, '—') AS Producto,
                                       m.fecha, m.cantidad, m.tipo
                        FROM Movimiento m
                        LEFT JOIN Producto p ON m.id_producto = p.id_producto
                        ORDER BY m.fecha DESC, m.id_movimiento DESC";

                    using (var cmd = new SqlCommand(sql, con))
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            model.Ultimos.Add(new InicioViewModel.UltimoMovimientoDto
                            {
                                Id = dr.GetInt32(0),
                                Producto = dr["Producto"]?.ToString() ?? "—",
                                Fecha = Convert.ToDateTime(dr["fecha"]),
                                Cantidad = Convert.ToInt32(dr["cantidad"]),
                                Tipo = dr["tipo"]?.ToString() ?? ""
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // No bloquear la vista por errores: mostrar mensaje y devolver lo que haya
                TempData["Error"] = "No se pudieron cargar algunos datos de inicio: " + ex.Message;
            }

            return View(model);
        }
    }
}
