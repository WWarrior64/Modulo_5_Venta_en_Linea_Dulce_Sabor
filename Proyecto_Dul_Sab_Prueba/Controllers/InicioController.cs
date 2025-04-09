using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class InicioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
