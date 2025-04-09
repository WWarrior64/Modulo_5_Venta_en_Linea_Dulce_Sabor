using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Proyecto_Dul_Sab_Prueba.Models;
using Proyecto_Dul_Sab_Prueba.Servicios;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{

    public class LoginController : Controller
    {
        private readonly ventaDbContext _ventaDbContext;

        public LoginController(ventaDbContext context)
        {
            _ventaDbContext = context;
        }

        [Autenticacion]
        public IActionResult Index()
        {
            var nombreUsuario = HttpContext.Session.GetString("Nombre");

            //Retorno información a la vista por ViewBag y ViewData
            ViewData["nombre"] = nombreUsuario;
            return View();
        }

        public IActionResult Autenticar()
        {
            ViewData["ErrorMessage"] = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Autenticar(string txtemail, string txtpassword)
        {
            //Valido al usuario con la base de datos
            var usuario = (from u in _ventaDbContext.Clientes
                           where u.correo == txtemail
                           && u.contraseña == txtpassword
                           select u).FirstOrDefault();

            //Si el usuario existe con todas sus validaciones
            if (usuario != null)
            {
                //Se crean las variables de sesión
                HttpContext.Session.SetInt32("UsuarioId", usuario.clienteId);
                HttpContext.Session.SetString("Nombre", usuario.nombre);
                HttpContext.Session.SetString("Correo", usuario.correo);

                //Se redirecciona al método de Index del controlador Home
                return RedirectToAction("Index", "Inicio");
            }

            ViewData["ErrorMessage"] = "Error, usuario invalido!!!!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Autenticar", "Login");
        }
    }
}
