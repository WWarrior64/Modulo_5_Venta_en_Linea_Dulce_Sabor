using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Dul_Sab_Prueba.Models;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ventaDbContext _ventaDbContext;

        public CarritoController(ventaDbContext context)
        {
            _ventaDbContext = context;
        }

        public IActionResult Index()
        {
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

            ViewData["listadoCarrito"] = productosEnCarrito;
            ViewData["Subtotal"] = productosEnCarrito.Sum(p => (p.platilloPrecio + p.comboPrecio + p.promocionPrecio - p.promocionDesc) * p.cantidad);
            ViewData["Total"] = ViewData["Subtotal"];

            return View();
        }

        [HttpPost]
        public IActionResult Checkout(string couponCode)
        {
            // Simulación de lógica para aplicar cupón
            if (!string.IsNullOrEmpty(couponCode))
            {
                ViewData["MensajeDescuento"] = "Cupón aplicado correctamente. Descuento activado.";
            }

            return RedirectToAction("Pedido", "Pedido");
        }


        [HttpPost]
        public IActionResult ActualizarCarrito(int[] ItemId, int[] Cantidad)
        {
            for (int i = 0; i < ItemId.Length; i++)
            {
                var producto = _ventaDbContext.Item.FirstOrDefault(p => p.ItemId == ItemId[i]);
                if (producto != null)
                {
                    producto.cantidad = Cantidad[i];
                    _ventaDbContext.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveItem(int id)
        {
            var item = _ventaDbContext.Item.FirstOrDefault(i => i.ItemId == id);
            if (item != null)
            {
                _ventaDbContext.Item.Remove(item);
                _ventaDbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
