using Microsoft.AspNetCore.Mvc;
using Proyecto_Dul_Sab_Prueba.Models;
using System.Diagnostics;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var usuarioCorreo = HttpContext.Session.GetString("Correo");

            // Pasar el correo a la vista principal
            ViewBag.UsuarioCorreo = usuarioCorreo;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
