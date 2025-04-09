using Microsoft.AspNetCore.Mvc;
/*using VIstas_de_pago_y_fatura.Models;

namespace VIstas_de_pago_y_fatura.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult Invoice()
        {
            if (TempData["Pedido"] == null)
            {
                return BadRequest("No se recibieron datos del pedido.");
            }

            var pedido = System.Text.Json.JsonSerializer.Deserialize<Pedido>(TempData["Pedido"].ToString());
            pedido.ResumenPedido ??= new List<ItemPedido>(); // Evita NullReferenceException

            ViewBag.PagoEfectivo = (bool)TempData["PagoEfectivo"];

            return View("Invoice", pedido);
        }

    }
}
*/