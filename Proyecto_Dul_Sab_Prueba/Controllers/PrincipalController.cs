using Microsoft.AspNetCore.Mvc;
using Proyecto_Dul_Sab_Prueba.Models;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly ventaDbContext _ventaDbContext;

        public PrincipalController(ventaDbContext context)
        {
            _ventaDbContext = context;
        }
        public IActionResult LayoutPrincipal()
        {
            return View();
        }
    }
}
