using Microsoft.AspNetCore.Mvc;

namespace InveDB.Controles
{
    public class InicioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
