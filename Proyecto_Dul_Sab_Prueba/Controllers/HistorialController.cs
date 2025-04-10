using Microsoft.AspNetCore.Mvc;
using Proyecto_Dul_Sab_Prueba.Models;
using System.Linq;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class HistorialController : Controller
    {
        private readonly ventaDbContext _ventaDbContext;

        public HistorialController(ventaDbContext context)
        {
            _ventaDbContext = context;
        }

        public IActionResult Index()
        {
            var UsuarioId = HttpContext.Session.GetInt32("UsuarioId");

            var pedidosEntregados = (from pedido in _ventaDbContext.Pedido_En_Linea
                                     join metodo in _ventaDbContext.Metodo_Pago on pedido.metodopagoId equals metodo.metodopagoId
                                     where pedido.estado == "Entregado" && pedido.clienteId == UsuarioId
                                     select new
                                     {
                                         pedido.pedidoId,
                                         pedido.fechapedido,
                                         pedido.metodopagoId,
                                         metodoPagoNombre = metodo.nombre,
                                         pedido.total,
                                         detalles = (from detalle in _ventaDbContext.Detalle_Pedido_En_Linea
                                                     where detalle.pedidoId == pedido.pedidoId
                                                     join i in _ventaDbContext.Item on detalle.itemId equals i.ItemId into itemJoin
                                                     from i in itemJoin.DefaultIfEmpty()

                                                     join p in _ventaDbContext.Platillo on i.platilloId equals p.platilloId into platilloJoin
                                                     from p in platilloJoin.DefaultIfEmpty()

                                                     join c in _ventaDbContext.Combo on i.comboId equals c.comboId into comboJoin
                                                     from c in comboJoin.DefaultIfEmpty()

                                                     join pr in _ventaDbContext.Promocion on i.promocionId equals pr.promocionId into promoJoin
                                                     from pr in promoJoin.DefaultIfEmpty()

                                                     select new
                                                     {
                                                         detalle.itemId,
                                                         tipo = detalle.tipo,
                                                         nombreProducto = detalle.tipo == "platillo" ? p.nombre :
                                                                          detalle.tipo == "combo" ? c.nombre :
                                                                          detalle.tipo == "promocion" ? pr.nombre : "Desconocido",
                                                         descripcion = detalle.tipo == "platillo" ? p.descripcion :
                                                                        detalle.tipo == "combo" ? c.descripcion :
                                                                        detalle.tipo == "promocion" ? pr.descripcion : "Sin descripción",
                                                         cantidad = detalle.cantidad,
                                                         preciounitario = detalle.preciounitario,
                                                         subtotal = detalle.subtotal
                                                     }).ToList()
                                     }).ToList();

            ViewData["listadoDePedidos"] = pedidosEntregados;
            return View();
        }
    }
}
