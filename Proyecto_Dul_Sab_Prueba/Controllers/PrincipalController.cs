using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class PrincipalController : Controller
    {
        public IActionResult LayoutPrincipal()
        {
            var usuarioCorreo = HttpContext.Session.GetString("Correo");

            ViewData["UsuarioCorreo"] = usuarioCorreo;

            return View();
        }
    }
}
