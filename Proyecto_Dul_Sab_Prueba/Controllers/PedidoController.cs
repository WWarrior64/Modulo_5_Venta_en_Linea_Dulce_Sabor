using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Dul_Sab_Prueba.Models;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class PedidoController : Controller
    {
        private readonly ventaDbContext _ventaDbContext;

        public PedidoController(ventaDbContext context)
        {
            _ventaDbContext = context;
        }

        public async Task<IActionResult> Pedido()
        {
            var usuarioNombre = HttpContext.Session.GetString("Correo");
            var nombre = HttpContext.Session.GetString("Nombre");

            // Verificar si el usuario está autenticado
            if (string.IsNullOrEmpty(usuarioNombre))
            {
                return RedirectToAction("Autenticar", "Login");
            }

            // Obtener el clienteId usando el nombre de usuario (suponiendo que el nombre de usuario es el correo)
            var cliente = _ventaDbContext.Clientes.FirstOrDefault(c => c.correo == usuarioNombre);
            if (cliente == null)
            {
                return RedirectToAction("Autenticar", "Login");
            }

            var clienteId = cliente.clienteId;

            var ubicacion = _ventaDbContext.Ubicacion_Geografica.FirstOrDefault(u => u.clienteId == cliente.clienteId);

            ViewData["Direccion"] = cliente.direccion;
            ViewData["Ciudad"] = ubicacion.ciudad;
            ViewData["Departamento"] = ubicacion.departamento;

            // Obtener los productos en el carrito
            var productosEnCarrito = (from i in _ventaDbContext.Item
                                        where i.estado != null && i.estado.Trim().ToLower() == "carrito"

                                        join p in _ventaDbContext.Platillo
                                            on i.platilloId equals p.platilloId into platilloJoin
                                        from p in platilloJoin.DefaultIfEmpty()

                                        join c in _ventaDbContext.Combo
                                            on i.comboId equals c.comboId into comboJoin
                                        from c in comboJoin.DefaultIfEmpty()

                                        join pr in _ventaDbContext.Promocion
                                            on i.promocionId equals pr.promocionId into promoJoin
                                        from pr in promoJoin.DefaultIfEmpty()

                                        select new
                                        {
                                            i.ItemId,
                                            i.platilloId,
                                            platilloDescrip = p.descripcion,
                                            nombrePlat = p.nombre,
                                            i.comboId,
                                            comboDescrip = c.descripcion,
                                            nombreComb = c.nombre,
                                            i.promocionId,
                                            promocionDescrip = pr.descripcion,
                                            nombreProm = pr.nombre,
                                            platilloPrecio = p != null ? p.precio : 0,
                                            comboPrecio = c != null ? c.precio : 0,
                                            promocionPrecio = pr != null ? pr.precio : 0,
                                            promocionDesc = pr != null ? pr.descuento : 0,
                                            platilloImg = p.imgUrl,
                                            comboImg = c.imgUrl,
                                            promocionImg = pr.imgUrl,
                                            i.cantidad
                                        })
                                      .ToList();

            var metodosPago = _ventaDbContext.Metodo_Pago.Select(mp => new { mp.metodopagoId, mp.nombre }).ToList();
            ViewData["MetodosPago"] = new SelectList(metodosPago, "metodopagoId", "nombre");

            ViewData["usuario"] = nombre;
            ViewData["correo"] = usuarioNombre;

            // Pasar los datos del carrito a la vista del pedido
            ViewData["listadoCarrito"] = productosEnCarrito;

            ViewData["Subtotal"] = productosEnCarrito.Sum(p => (p.platilloPrecio + p.comboPrecio + p.promocionPrecio - p.promocionDesc) * p.cantidad);
            ViewData["Total"] = ViewData["Subtotal"];

            // Obtener métodos de pago para seleccionar en el formulario


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearPedido(int metodopagoId)
        {
            var usuarioNombre = HttpContext.Session.GetString("Correo");

            // Obtener el clienteId usando el nombre de usuario (supuesto que el nombre de usuario es el correo)
            var cliente = _ventaDbContext.Clientes.FirstOrDefault(c => c.correo == usuarioNombre);
            if (cliente == null)
            {
                return RedirectToAction("Autenticar", "Login");
            }

            // Obtener el clienteId del usuario autenticado
            var clienteId = cliente.clienteId;

            // Obtener los productos del carrito
            var productosEnCarrito = (from i in _ventaDbContext.Item
                                      where i.estado != null && i.estado.Trim().ToLower() == "carrito"

                                      join p in _ventaDbContext.Platillo
                                          on i.platilloId equals p.platilloId into platilloJoin
                                      from p in platilloJoin.DefaultIfEmpty()

                                      join c in _ventaDbContext.Combo
                                          on i.comboId equals c.comboId into comboJoin
                                      from c in comboJoin.DefaultIfEmpty()

                                      join pr in _ventaDbContext.Promocion
                                          on i.promocionId equals pr.promocionId into promoJoin
                                      from pr in promoJoin.DefaultIfEmpty()

                                      select new
                                      {
                                          i.ItemId,
                                          i.platilloId,
                                          i.comboId,
                                          i.promocionId,
                                          nombrePlat = p.nombre,
                                          nombreComb = c.nombre,
                                          nombreProm = pr.nombre,
                                          platilloPrecio = p != null ? p.precio : 0,
                                          comboPrecio = c != null ? c.precio : 0,
                                          promocionPrecio = pr != null ? pr.precio : 0,
                                          promocionDesc = pr != null ? pr.descuento : 0,
                                          i.cantidad
                                      })
                                      .ToList();

            decimal subtotal = productosEnCarrito.Sum(p => (p.platilloPrecio + p.comboPrecio + p.promocionPrecio - p.promocionDesc) * p.cantidad);
            decimal total = subtotal;

            // Crear el pedido
            Pedido_en_Linea nuevoPedido = new Pedido_en_Linea
            {
                clienteId = clienteId,
                fechapedido = DateTime.Now,
                estado = "Pendiente", // Estado inicial
                metodopagoId = metodopagoId,
                total = total,
                facturado = false, // No facturado por defecto
                fechafactura = null, // Aún no tiene fecha de factura
            };

            _ventaDbContext.Pedido_En_Linea.Add(nuevoPedido);
            _ventaDbContext.SaveChanges();

            // Crear los detalles del pedido
            foreach (var item in productosEnCarrito)
            {
                Detalle_Pedido_en_Linea detalle = new Detalle_Pedido_en_Linea
                {
                    pedidoId = nuevoPedido.pedidoId,
                    itemId = item.ItemId,
                    tipo = item.platilloId != null ? "platillo" :
                           item.comboId != null ? "combo" :
                           item.promocionId != null ? "promocion" : null,
                    cantidad = item.cantidad,
                    preciounitario = item.platilloPrecio + item.comboPrecio + item.promocionPrecio - item.promocionDesc
                };

                _ventaDbContext.Detalle_Pedido_En_Linea.Add(detalle);
            }

            _ventaDbContext.SaveChanges();

            // Limpiar el carrito (opcional)
            foreach (var item in productosEnCarrito)
            {
                var carritoItem = _ventaDbContext.Item.FirstOrDefault(i => i.ItemId == item.ItemId);
                if (carritoItem != null)
                {
                    carritoItem.estado = "pedido";  // Cambiar el estado del artículo para reflejar que ha sido comprado
                }
            }

            _ventaDbContext.SaveChanges();

            // Redirigir a la vista de confirmación o éxito
            return RedirectToAction("Invoice", "Invoice", new { pedidoId = nuevoPedido.pedidoId });
        }
    }
}
