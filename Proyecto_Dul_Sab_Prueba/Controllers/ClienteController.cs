using Microsoft.AspNetCore.Mvc;
using Proyecto_Dul_Sab_Prueba.Models;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ventaDbContext _ventaDbContext;

        public ClienteController(ventaDbContext context)
        {
            _ventaDbContext = context;
        }

        public IActionResult Index()
        {
            var nombreUsuario = HttpContext.Session.GetString("Nombre");
            var correoUsuario = HttpContext.Session.GetString("Correo");
            var telefonoUsuario = HttpContext.Session.GetString("Telefono");
            var direccionUsuario = HttpContext.Session.GetString("Direccion");

            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(correoUsuario))
            {
                return RedirectToAction("Autenticar", "Login");
            }

            ViewData["Nombre"] = nombreUsuario;
            ViewData["Correo"] = correoUsuario;
            ViewData["Telefono"] = telefonoUsuario;
            ViewData["Direccion"] = direccionUsuario;

            return View();
        }

        [HttpPost]
        public IActionResult ActualizarCliente(Clientes actualizarCliente)
        {
            var UsuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var cliente = _ventaDbContext.Clientes.FirstOrDefault(c => c.clienteId == UsuarioId);

            if (cliente != null)
            {
                cliente.nombre = actualizarCliente.nombre;
                cliente.telefono = actualizarCliente.telefono;
                cliente.direccion = actualizarCliente.direccion;

                _ventaDbContext.SaveChanges();
            }

            return RedirectToAction("Index", "Cliente");
        }

    }
}
