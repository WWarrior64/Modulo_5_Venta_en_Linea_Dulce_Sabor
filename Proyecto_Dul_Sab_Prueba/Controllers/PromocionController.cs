using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Dul_Sab_Prueba.Models;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class PromocionController : Controller
    {
        private readonly ventaDbContext _ventaDbContext;

        public PromocionController(ventaDbContext ventaDbContext)
        {
            _ventaDbContext = ventaDbContext;
        }

        public IActionResult Index()
        {
            //Para calcular el tiempo de las promociones
            var currentDate = DateTime.Now;

            var listadoDePromocion = (from p in _ventaDbContext.Promocion
                                      select new
                                      {
                                          p.promocionId,
                                          nombre = p.nombre,
                                          descripcion = p.descripcion,
                                          imgUrl = p.imgUrl,
                                          tipo = p.tipo,
                                          precio = p.precio,
                                          descuento = p.descuento,
                                          fechainicio = p.fechainicio,
                                          fechafin = p.fechafin
                                      }).ToList();

            ViewData["listadoDePromocion"] = listadoDePromocion;

            return View();
        }

        [HttpPost]
        public IActionResult AñadirItem(int promocionId, decimal descuento, decimal precio, string nombre)
        {

            var item = new Item
            {
                promocionId = promocionId,
                cantidad = 1,
                estado = "carrito",
            };

            // Se agrega la promoción al carrito
            _ventaDbContext.Item.Add(item);
            _ventaDbContext.SaveChanges();

            // Redirige al índice de promociones después de agregar el item
            return RedirectToAction("Index", "Promocion");
        }
    }
}
