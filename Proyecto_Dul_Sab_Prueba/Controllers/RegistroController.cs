using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Dul_Sab_Prueba.Models;
using System.Linq;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class RegistroController : Controller
    {
        private readonly ventaDbContext _ventaDbContext;

        public RegistroController(ventaDbContext context)
        {
            _ventaDbContext = context;
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Clientes nuevoCliente, Ubicacion_Geografica nuevaUbicacion)
        {
            var clienteExistente = _ventaDbContext.Clientes.Any(c => c.correo == nuevoCliente.correo);
            if (clienteExistente)
            {
                ViewData["ErrorCorreo"] = "Este correo ya está registrado.";
                return View();
            }
            _ventaDbContext.Clientes.Add(nuevoCliente);
            _ventaDbContext.SaveChanges();

            nuevaUbicacion.clienteId = nuevoCliente.clienteId;
            _ventaDbContext.Ubicacion_Geografica.Add(nuevaUbicacion);
            _ventaDbContext.SaveChanges();

                return RedirectToAction("Autenticar", "Login");
        }

    }
}
