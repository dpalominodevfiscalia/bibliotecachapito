using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace BibliotecaDB.Controllers
{
    public class PedidosVentaController : BaseController
    {
        public PedidosVentaController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: PedidosVenta
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreatePedidoPermission"] = HasOpcionActionPermission("PedidosVenta", "Crear");
            HttpContext.Items["HasEditPedidoPermission"] = HasOpcionActionPermission("PedidosVenta", "Editar");
            HttpContext.Items["HasDetailsPedidoPermission"] = HasOpcionActionPermission("PedidosVenta", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeletePedidoPermission"] = HasOpcionActionPermission("PedidosVenta", "Eliminar");
            HttpContext.Items["HasTogglePedidoPermission"] = HasOpcionActionPermission("PedidosVenta", "Desactivar");

            var pedidos = _dataService.GetPedidosVenta();
            foreach (var pedido in pedidos)
            {
                if (pedido.IdCliente.HasValue)
                {
                    pedido.Cliente = _dataService.GetClienteById(pedido.IdCliente.Value);
                }
                if (pedido.IdCotizacion.HasValue)
                {
                    pedido.Cotizacion = _dataService.GetCotizacionById(pedido.IdCotizacion.Value);
                }
            }

            return View(pedidos);
        }

        // GET: PedidosVenta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var pedido = _dataService.GetPedidoVentaById(id.Value);
            if (pedido == null)
            {
                return NotFound();
            }

            // Load related entities
            if (pedido.IdCliente.HasValue)
            {
                pedido.Cliente = _dataService.GetClienteById(pedido.IdCliente.Value);
            }
            if (pedido.IdCotizacion.HasValue)
            {
                pedido.Cotizacion = _dataService.GetCotizacionById(pedido.IdCotizacion.Value);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", pedido);
            }
            return View(pedido);
        }

        // GET: PedidosVenta/Create
        public ActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_dataService.GetClientes(), "Id", "NombreCompleto");
            ViewData["IdCotizacion"] = new SelectList(_dataService.GetCotizaciones(), "Id", "NumeroCotizacion");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: PedidosVenta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,NumeroPedido,IdCliente,IdCotizacion,FechaPedido,FechaEntrega,CondicionesPago,Moneda,Subtotal,Impuestos,Total,Observaciones,Estado,Vendedor")] PedidoVenta pedido)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddPedidoVenta(pedido);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Pedido de venta creado exitosamente" });
                }
                return RedirectToAction("Index");
            }

            ViewData["IdCliente"] = new SelectList(_dataService.GetClientes(), "Id", "NombreCompleto", pedido.IdCliente);
            ViewData["IdCotizacion"] = new SelectList(_dataService.GetCotizaciones(), "Id", "NumeroCotizacion", pedido.IdCotizacion);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", pedido);
            }
            return View(pedido);
        }

        // GET: PedidosVenta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var pedido = _dataService.GetPedidoVentaById(id.Value);
            if (pedido == null)
            {
                return NotFound();
            }

            ViewData["IdCliente"] = new SelectList(_dataService.GetClientes(), "Id", "NombreCompleto", pedido.IdCliente);
            ViewData["IdCotizacion"] = new SelectList(_dataService.GetCotizaciones(), "Id", "NumeroCotizacion", pedido.IdCotizacion);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", pedido);
            }
            return View(pedido);
        }

        // POST: PedidosVenta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,NumeroPedido,IdCliente,IdCotizacion,FechaPedido,FechaEntrega,CondicionesPago,Moneda,Subtotal,Impuestos,Total,Observaciones,Estado,Vendedor")] PedidoVenta pedido)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdatePedidoVenta(pedido);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Pedido de venta actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }

            ViewData["IdCliente"] = new SelectList(_dataService.GetClientes(), "Id", "NombreCompleto", pedido.IdCliente);
            ViewData["IdCotizacion"] = new SelectList(_dataService.GetCotizaciones(), "Id", "NumeroCotizacion", pedido.IdCotizacion);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", pedido);
            }
            return View(pedido);
        }

        // GET: PedidosVenta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var pedido = _dataService.GetPedidoVentaById(id.Value);
            if (pedido == null)
            {
                return NotFound();
            }

            // Load related entities for display
            if (pedido.IdCliente.HasValue)
            {
                pedido.Cliente = _dataService.GetClienteById(pedido.IdCliente.Value);
            }
            if (pedido.IdCotizacion.HasValue)
            {
                pedido.Cotizacion = _dataService.GetCotizacionById(pedido.IdCotizacion.Value);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", pedido);
            }
            return View(pedido);
        }

        // POST: PedidosVenta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeletePedidoVenta(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Pedido de venta eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.TogglePedidoVentaEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}