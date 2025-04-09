using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Dul_Sab_Prueba.Models;

namespace Proyecto_Dul_Sab_Prueba.Controllers
{
    public class PlatilloController : Controller
    {
        private readonly ventaDbContext _ventaDbContext;

        public PlatilloController(ventaDbContext ventaDbContext)
        {
            _ventaDbContext = ventaDbContext;
        }

        public IActionResult Index(int? categoriaId, int? subcategoriaId)
        {
            // Obtener las listas de categorías y subcategorías
            var listaDeCategorias = _ventaDbContext.Categoria.ToList();
            var listaDeSubCategorias = _ventaDbContext.SubCategoria.ToList();

            ViewData["listadoDeCategorias"] = new SelectList(listaDeCategorias, "categoriaId", "nombre", categoriaId);
            ViewData["listadoDeSubCategorias"] = new SelectList(listaDeSubCategorias, "subcategoriaId", "nombre", subcategoriaId);

            // Consulta para obtener los platillos con los nombres de categoría y subcategoría
            var listadoDePlatillos = from p in _ventaDbContext.Platillo
                                     join ca in _ventaDbContext.Categoria
                                       on p.categoriaId equals ca.categoriaId
                                     join sc in _ventaDbContext.SubCategoria
                                       on p.subcategoriaId equals sc.subcategoriaId into sub
                                     from sc in sub.DefaultIfEmpty() // Permite que la subcategoría sea opcional
                                     select new
                                     {
                                         p.platilloId,
                                         nombre = p.nombre,
                                         descripcion = p.descripcion,
                                         imgUrl = p.imgUrl,
                                         categoriaNombre = ca.nombre,
                                         subcategoriaNombre = sc != null ? sc.nombre : null,
                                         precio = p.precio,
                                         disponible = p.disponible,
                                         categoriaId = p.categoriaId,
                                         subcategoriaId = p.subcategoriaId
                                     };

            // Aplicar filtros si se seleccionaron
            if (categoriaId.HasValue && categoriaId > 0)
            {
                listadoDePlatillos = listadoDePlatillos.Where(p => p.categoriaId == categoriaId);
            }

            if (subcategoriaId.HasValue && subcategoriaId > 0)
            {
                listadoDePlatillos = listadoDePlatillos.Where(p => p.subcategoriaId == subcategoriaId);
            }

            ViewData["listadoDePlatillos"] = listadoDePlatillos.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AñadirItem(int platilloId, decimal precio, string nombre)
        {

            var item = new Item
            {
                platilloId = platilloId,
                cantidad = 1,
                estado = "carrito"
            };

            _ventaDbContext.Item.Add(item);
            _ventaDbContext.SaveChanges();

            return RedirectToAction("Index", "Platillo");
        }
    }
}
