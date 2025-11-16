using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using InveDB.Models;

namespace InveDB.Controles
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        // ------------------------------------------------------
        // ACCEDER (usa tabla Usuarios y luego fallback a conexiones)
        // ------------------------------------------------------
        [HttpPost]
        public IActionResult Acceder(string usuario, string password)
        {
            // 🔹 1. Intentar validar con la tabla Usuarios
            Usuario user = ValidarUsuario(usuario, password);

            if (user != null)
            {
                // Guardar sesión
                HttpContext.Session.SetInt32("IdUsuario", user.Id);
                HttpContext.Session.SetString("Usuario", user.UsuarioNombre);
                HttpContext.Session.SetString("Rol", user.Rol);
                HttpContext.Session.SetString("ConexionActiva",
                    _config.GetConnectionString("DefaultConnection"));

                // Redirección
                return RedirigirPorRol(user.Rol);
            }

            // 🔹 2. Si no existe en la tabla, intentar CONEXIONES PERSONALIZADAS
            string conexionAlterna = GetConnectionByUser(usuario, password);

            if (conexionAlterna == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View("Login");
            }

            // Detectar rol por nombre de usuario
            string rolAlterno = DetectRole(usuario);

            // Guardar sesión
            HttpContext.Session.SetString("Usuario", usuario);
            HttpContext.Session.SetString("Rol", rolAlterno);
            HttpContext.Session.SetString("ConexionActiva", conexionAlterna);

            // Redirigir
            return RedirigirPorRol(rolAlterno);
        }

        // ------------------------------------------------------
        // LOGOUT
        // ------------------------------------------------------
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        // ------------------------------------------------------
        // VALIDAR USUARIO REAL EN TABLA
        // ------------------------------------------------------
        private Usuario ValidarUsuario(string usuario, string password)
        {
            string conn = _config.GetConnectionString("DefaultConnection");

            using SqlConnection connection = new(conn);
            connection.Open();

            string sql = @"
                SELECT Id, Usuario, PasswordHash, Rol, Activo, FechaCreacion
                FROM Usuarios
                WHERE Usuario = @user AND Activo = 1";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@user", usuario);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            string passBD = reader["PasswordHash"].ToString();

            // Comparación simple (sin hash)
            if (passBD != password)
                return null;

            return new Usuario
            {
                Id = (int)reader["Id"],
                UsuarioNombre = reader["Usuario"].ToString(),
                PasswordHash = passBD,
                Rol = reader["Rol"].ToString(),
                Activo = (bool)reader["Activo"],
                FechaCreacion = (DateTime)reader["FechaCreacion"]
            };
        }

        // ------------------------------------------------------
        // SOPORTE PARA TUS CONEXIONES PERSONALIZADAS
        // ------------------------------------------------------
        private string GetConnectionByUser(string usuario, string pass)
        {
            if (usuario == "admin_login" && pass == "1234")
                return _config.GetConnectionString("Conexion_Admin");

            if (usuario == "encargado_login" && pass == "1234")
                return _config.GetConnectionString("Conexion_Encargado");

            if (usuario == "capturista_login" && pass == "1234")
                return _config.GetConnectionString("Conexion_Capturista");

            return null;
        }

        private string DetectRole(string usuario)
        {
            return usuario switch
            {
                "admin_login" => "Admin",
                "encargado_login" => "Encargado",
                "capturista_login" => "Capturista",
                _ => "Capturista"
            };
        }

        // ------------------------------------------------------
        // REDIRECCIÓN POR ROL
        // ------------------------------------------------------
        private IActionResult RedirigirPorRol(string rol)
        {
            return rol switch
            {
                "Admin" => RedirectToAction("Index", "Inicio"),
                "Encargado" => RedirectToAction("Index", "Inventario"),
                "Capturista" => RedirectToAction("Index", "Movimientos"),
                _ => RedirectToAction("Index", "Login")
            };
        }
    }
}
