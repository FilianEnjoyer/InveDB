using InveDB.Datos;
using InveDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace InveDB.Controles
{
    public class UsuariosController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly EjecutarCmdWeb _cmd;

        public UsuariosController(IConfiguration configuration, EjecutarCmdWeb cmd, IHttpContextAccessor context)
        {
            _configuration = configuration;
            _cmd = cmd;
            _context = context;
        }

        // -------------------------------------------
        //  LISTADO
        // -------------------------------------------
        public IActionResult Index()
        {
            List<Usuario> lista = new();

            string connectionString = _context.HttpContext.Session.GetString("ConexionActiva")
                ?? _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new(connectionString))
            {
                string query = @"
                    SELECT Id, Usuario, PasswordHash, Rol, Activo, FechaCreacion
                    FROM Usuarios
                    ORDER BY Usuario ASC";

                using (SqlCommand cmd = new(query, con))
                {
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario
                            {
                                Id = (int)dr["Id"],
                                UsuarioNombre = dr["Usuario"].ToString(),
                                PasswordHash = dr["PasswordHash"].ToString(),
                                Rol = dr["Rol"].ToString(),
                                Activo = (bool)dr["Activo"],
                                FechaCreacion = (DateTime)dr["FechaCreacion"]
                            });
                        }
                    }
                }
            }

            return View(lista);
        }


        // -------------------------------------------
        //  CREAR
        // -------------------------------------------
        [HttpPost]
        public IActionResult Crear(string usuario, string password, string rol)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(rol))
                {
                    TempData["Error"] = "Todos los campos son obligatorios.";
                    return RedirectToAction("Index");
                }

                string sql = $@"
                    INSERT INTO Usuarios (Usuario, PasswordHash, Rol, Activo, FechaCreacion)
                    VALUES ('{usuario}', '{password}', '{rol}', 1, GETDATE())";

                _cmd.EjecutarNonQuery(sql);

                TempData["Mensaje"] = "Usuario creado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al crear usuario: " + ex.Message;
            }

            return RedirectToAction("Index");
        }


        // -------------------------------------------
        //  OBTENER 1 USUARIO (PARA EDITAR)
        // -------------------------------------------
        public IActionResult Obtener(int id)
        {
            string connectionString = _context.HttpContext.Session.GetString("ConexionActiva")
                ?? _configuration.GetConnectionString("DefaultConnection");

            Usuario user = null;

            using (SqlConnection con = new(connectionString))
            {
                string sql = @"SELECT Id, Usuario, PasswordHash, Rol, Activo, FechaCreacion 
                       FROM Usuarios 
                       WHERE Id = @id";

                using (SqlCommand cmd = new(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            user = new Usuario
                            {
                                Id = dr.GetInt32(dr.GetOrdinal("Id")),
                                UsuarioNombre = dr["Usuario"]?.ToString(),
                                PasswordHash = dr["PasswordHash"]?.ToString(),
                                Rol = dr["Rol"]?.ToString(),
                                Activo = dr["Activo"] != DBNull.Value && Convert.ToBoolean(dr["Activo"]),
                                FechaCreacion = dr["FechaCreacion"] != DBNull.Value
                                    ? Convert.ToDateTime(dr["FechaCreacion"])
                                    : DateTime.MinValue
                            };
                        }
                    }
                }
            }

            if (user == null)
                return Json(new { success = false, message = "Usuario no encontrado" });

            return Json(new { success = true, data = user });
        }



        // -------------------------------------------
        //  EDITAR
        // -------------------------------------------
        [HttpPost]
        public IActionResult Editar(int id, string usuario, string password, string rol, bool activo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario) ||
                    string.IsNullOrWhiteSpace(rol))
                {
                    TempData["Error"] = "Datos inválidos.";
                    return RedirectToAction("Index");
                }

                string sql = $@"
                    UPDATE Usuarios
                    SET Usuario = '{usuario}',
                        PasswordHash = '{password}',
                        Rol = '{rol}',
                        Activo = {(activo ? 1 : 0)}
                    WHERE Id = {id}";

                _cmd.EjecutarNonQuery(sql);

                TempData["Mensaje"] = "Usuario actualizado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al editar usuario: " + ex.Message;
            }

            return RedirectToAction("Index");
        }


        // -------------------------------------------
        //  ELIMINAR
        // -------------------------------------------
        public IActionResult Eliminar(int id)
        {
            try
            {
                string sql = $"DELETE FROM Usuarios WHERE Id = {id}";

                _cmd.EjecutarNonQuery(sql);

                TempData["Mensaje"] = "Usuario eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar usuario: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
