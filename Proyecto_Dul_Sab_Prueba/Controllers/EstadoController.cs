using Microsoft.AspNetCore.Mvc;
using Proyecto_Dul_Sab_Prueba.Models;
using System.Linq;
using System.Collections.Generic;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class EstadoController : Controller
    {
        private readonly ventaDbContext _ventaDbContext;

        public EstadoController(ventaDbContext context)
        {
            _ventaDbContext = context;
        }

        public IActionResult Index()
        {
            var usuarioCorreo = HttpContext.Session.GetString("Correo");

            // 📌 Verificar autenticación
            if (string.IsNullOrEmpty(usuarioCorreo))
            {
                return RedirectToAction("Autenticar", "Login");
            }

            // 📌 Obtener el clienteId
            var cliente = _ventaDbContext.Clientes.FirstOrDefault(c => c.correo == usuarioCorreo);
            if (cliente == null)
            {
                return RedirectToAction("Autenticar", "Login");
            }

            // 📌 Obtener todos los pedidos del cliente
            var pedidos = _ventaDbContext.Pedido_En_Linea
                .Where(p => p.clienteId == cliente.clienteId &&
                            p.estado != "Entregado" && p.estado != "Cancelado")
                .OrderByDescending(p => p.fechapedido)
                .ToList();

            if (!pedidos.Any())
            {
                return View("SinPedidos"); // 📌 Si no hay pedidos, mostrar otra vista
            }

            ViewData["ListadoPedidos"] = pedidos;
            return View();
        }

        [HttpPost]
        public IActionResult Cancelar(int pedidoId)
        {
            // 📌 Buscar el pedido
            var pedido = _ventaDbContext.Pedido_En_Linea.FirstOrDefault(p => p.pedidoId == pedidoId);
            if (pedido == null || pedido.estado == "Cancelado")
            {
                return NotFound();
            }

            // 📌 Cambiar el estado a "Cancelado"
            pedido.estado = "Cancelado";
            _ventaDbContext.SaveChanges();

            return RedirectToAction("Index"); // 📌 Recargar la vista para reflejar cambios
        }

        public IActionResult Actualizar()
        {
            return RedirectToAction("Index"); // 📌 Simplemente recarga los pedidos actualizados
        }
    }
}
