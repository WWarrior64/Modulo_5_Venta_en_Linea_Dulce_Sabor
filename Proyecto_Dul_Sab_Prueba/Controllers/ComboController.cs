using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Dul_Sab_Prueba.Models;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class ComboController : Controller
    {
        private readonly ventaDbContext _ventaDbContext;

        public ComboController(ventaDbContext ventaDbContext)
        {
            _ventaDbContext = ventaDbContext;
        }

        public IActionResult Index()
        {

            // Consulta con los nombres de categoria y subcategoria
            var listadoDeCombos = (from co in _ventaDbContext.Combo
                                      select new
                                      {
                                          co.comboId,
                                          nombre = co.nombre,
                                          descripcion = co.descripcion,
                                          imgUrl = co.imgUrl,
                                          precio = co.precio,
                                          disponible = co.disponible
                                      }).ToList();

            ViewData["listadoDeCombos"] = listadoDeCombos;

            return View();
        }

        [HttpPost]
        public IActionResult AñadirItem(int? comboId, decimal precio, string nombre)
        {
            if (!comboId.HasValue)
            {
               
                return RedirectToAction("Index");
            }

            var item = new Item
            {
                comboId = comboId,
                cantidad = 1,
                estado = "carrito"
            };

            _ventaDbContext.Item.Add(item);
            _ventaDbContext.SaveChanges();  // Guardamos el item en la base de datos

            // Redirige al carrito o la página de combos
            return RedirectToAction("Index", "Combo");  // O puedes redirigir a la vista de carrito
        }

    }
}
