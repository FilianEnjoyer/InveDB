using InveDB.Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// 1️ Registrar servicios
// ---------------------------

// Registrar el contexto de base de datos con la cadena de conexión
builder.Services.AddDbContext<ContextoAlmacen>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<EjecutarCmdWeb>();



// Agregar soporte para controladores y vistas MVC
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();
var app = builder.Build();

// ---------------------------
// 2️ Configurar la aplicación
// ---------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Activa seguridad HTTPS
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// ---------------------------
// 3️ Configurar rutas MVC
// ---------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Index}/{id?}");

// ---------------------------
// 4️ Ejecutar la aplicación
// ---------------------------
app.Run();
