using InveDB.Datos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// 1️ Registrar servicios
// ---------------------------

// Registrar el contexto de base de datos con la cadena de conexión por defecto
builder.Services.AddDbContext<ContextoAlmacen>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddSession();  // <- necesario para login
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<EjecutarCmdWeb>(provider =>
{
    var accessor = provider.GetRequiredService<IHttpContextAccessor>();
    var config = provider.GetRequiredService<IConfiguration>();

    // Tomar la conexión del rol desde la sesión
    string conn = accessor.HttpContext?.Session.GetString("ConexionActiva");

    if (string.IsNullOrEmpty(conn))
    {
        // Si no hay sesión, usar default para evitar errores
        conn = config.GetConnectionString("DefaultConnection");
    }

    return new EjecutarCmdWeb(conn);
});

builder.Services.AddScoped<ContextoAlmacen>(serviceProvider =>
{
    var http = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var config = serviceProvider.GetRequiredService<IConfiguration>();

    string conn =
        http.HttpContext?.Session.GetString("ConexionActiva")
        ?? config.GetConnectionString("DefaultConnection");

    var optionsBuilder = new DbContextOptionsBuilder<ContextoAlmacen>();
    optionsBuilder.UseSqlServer(conn);

    return new ContextoAlmacen(optionsBuilder.Options);
});



var app = builder.Build();

// ---------------------------
// 2️ Configurar la aplicación
// ---------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//  La sesión SIEMPRE debe ir antes de Authorization
app.UseSession();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? "";

    // --- PERMITIR ACCESO A LOGIN, LOGOUT, Y ESTÁTICOS ---
    if (path.StartsWith("/login") ||
        path.StartsWith("/cuenta") ||
        path.StartsWith("/logout") ||
        path.StartsWith("/css") ||
        path.StartsWith("/js") ||
        path.StartsWith("/lib") ||
        path.StartsWith("/images"))
    {
        await next();
        return;
    }
    // ----------------------------------------------------

    var config = context.RequestServices.GetRequiredService<IConfiguration>();
    var defaultConn = config.GetConnectionString("DefaultConnection");

    var sesionConexion = context.Session.GetString("ConexionActiva");

    // Sin sesión = usando DefaultConnection → bloquear
    if (string.IsNullOrEmpty(sesionConexion))
    {
        context.Session.Clear();
        context.Response.Redirect("/Login?default=1");
        return;
    }

    // Si está usando DefaultConnection → bloquear
    if (sesionConexion == defaultConn)
    {
        context.Session.Clear();
        context.Response.Redirect("/Login?default=1");
        return;
    }

    await next();
});

app.UseAuthorization();

app.MapRazorPages();

// ---------------------------
// 3️ Configurar rutas MVC
// ---------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

// ---------------------------
// 4️ Ejecutar la aplicación
// ---------------------------
app.Run();
