using Microsoft.AspNetCore.Mvc;
using Proyecto_Dul_Sab_Prueba.Models;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ventaDbContext _ventaDbContext;

        public InvoiceController(ventaDbContext context)
        {
            _ventaDbContext = context;
        }

        // Acción para mostrar la factura
        public async Task<IActionResult> Invoice(int pedidoId)
        {
            var usuarioNombre = HttpContext.Session.GetString("Correo");

            // Verificar si el usuario está autenticado
            if (string.IsNullOrEmpty(usuarioNombre))
            {
                return RedirectToAction("Autenticar", "Login");
            }

            // Obtener el clienteId usando el nombre de usuario
            var cliente = _ventaDbContext.Clientes.FirstOrDefault(c => c.correo == usuarioNombre);
            if (cliente == null)
            {
                return RedirectToAction("Autenticar", "Login");
            }

            var clienteId = cliente.clienteId;

            // Obtener el pedido por ID
            var pedido = (from p in _ventaDbContext.Pedido_En_Linea
                          where p.pedidoId == pedidoId && p.clienteId == clienteId
                          select new
                          {
                              PedidoId = p.pedidoId,
                              ClienteId = p.clienteId,
                              MetodoPago = p.metodopagoId,
                              Estado = p.estado,
                              FechaPedido = p.fechapedido,
                              Total = p.total
                          }).FirstOrDefault();

            var MetodoPago = _ventaDbContext.Metodo_Pago.FirstOrDefault(m => m.metodopagoId == pedido.MetodoPago);


            if (pedido == null)
            {
                return NotFound();
            }

            // Obtener los detalles del pedido
            var detallesPedido = (from dp in _ventaDbContext.Detalle_Pedido_En_Linea
                                  join i in _ventaDbContext.Item on dp.itemId equals i.ItemId
                                  join p in _ventaDbContext.Platillo on i.platilloId equals p.platilloId into platilloJoin
                                  from p in platilloJoin.DefaultIfEmpty()

                                  join c in _ventaDbContext.Combo on i.comboId equals c.comboId into comboJoin
                                  from c in comboJoin.DefaultIfEmpty()

                                  join pr in _ventaDbContext.Promocion on i.promocionId equals pr.promocionId into promoJoin
                                  from pr in promoJoin.DefaultIfEmpty()

                                  where dp.pedidoId == pedido.PedidoId
                                  select new
                                  {
                                      dp.detallepedidoId,
                                      Nombre = p != null ? p.nombre : c != null ? c.nombre : pr != null ? pr.nombre : "Desconocido",
                                      Descripcion = p != null ? p.descripcion : c != null ? c.descripcion : pr != null ? pr.descripcion : "Sin descripción",
                                      dp.cantidad,
                                      PrecioUnitario = p != null ? p.precio : c != null ? c.precio : pr != null ? pr.precio : 0,
                                      Descuento = pr != null ? pr.descuento : 0
                                  }).ToList();

            // Calcular totales
            decimal subtotal = detallesPedido.Sum(dp => (dp.PrecioUnitario - dp.Descuento) * dp.cantidad);
            decimal total = subtotal;

            // Pasar los datos a la vista
            ViewData["NombreCliente"] = cliente.nombre;
            ViewData["CorreoCliente"] = cliente.correo;
            ViewData["TelefonoCliente"] = cliente.telefono;
            ViewData["FechaPedido"] = pedido.FechaPedido;
            ViewData["Pedido"] = pedido;
            ViewData["PedidoId"] = pedido.PedidoId;
            ViewData["MetodoPago"] = MetodoPago.nombre;
            ViewData["DetallesPedido"] = detallesPedido;
            ViewData["Subtotal"] = subtotal;
            ViewData["Total"] = total;

            return View();
        }
    }
}
