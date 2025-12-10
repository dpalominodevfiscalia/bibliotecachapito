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
    public class CobranzasController : BaseController
    {
        public CobranzasController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Cobranzas
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateCobranzaPermission"] = HasOpcionActionPermission("Cobranzas", "Crear");
            HttpContext.Items["HasEditCobranzaPermission"] = HasOpcionActionPermission("Cobranzas", "Editar");
            HttpContext.Items["HasDetailsCobranzaPermission"] = HasOpcionActionPermission("Cobranzas", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteCobranzaPermission"] = HasOpcionActionPermission("Cobranzas", "Eliminar");
            HttpContext.Items["HasToggleCobranzaPermission"] = HasOpcionActionPermission("Cobranzas", "Desactivar");

            var cobranzas = _dataService.GetCobranzas();
            foreach (var cobranza in cobranzas)
            {
                if (cobranza.IdCliente.HasValue)
                {
                    cobranza.Cliente = _dataService.GetClienteById(cobranza.IdCliente.Value);
                }
                if (cobranza.IdPedidoVenta.HasValue)
                {
                    cobranza.PedidoVenta = _dataService.GetPedidoVentaById(cobranza.IdPedidoVenta.Value);
                }
            }

            return View(cobranzas);
        }

        // GET: Cobranzas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var cobranza = _dataService.GetCobranzaById(id.Value);
            if (cobranza == null)
            {
                return NotFound();
            }

            // Load related entities
            if (cobranza.IdCliente.HasValue)
            {
                cobranza.Cliente = _dataService.GetClienteById(cobranza.IdCliente.Value);
            }
            if (cobranza.IdPedidoVenta.HasValue)
            {
                cobranza.PedidoVenta = _dataService.GetPedidoVentaById(cobranza.IdPedidoVenta.Value);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", cobranza);
            }
            return View(cobranza);
        }

        // GET: Cobranzas/Create
        public ActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_dataService.GetClientes(), "Id", "NombreCompleto");
            ViewData["IdPedidoVenta"] = new SelectList(_dataService.GetPedidosVenta(), "Id", "NumeroPedido");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Cobranzas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,NumeroRecibo,IdCliente,IdPedidoVenta,FechaCobranza,Moneda,Monto,FormaPago,ReferenciaPago,Observaciones,Estado,Cobrador")] Cobranza cobranza)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddCobranza(cobranza);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Cobranza creada exitosamente" });
                }
                return RedirectToAction("Index");
            }

            ViewData["IdCliente"] = new SelectList(_dataService.GetClientes(), "Id", "NombreCompleto", cobranza.IdCliente);
            ViewData["IdPedidoVenta"] = new SelectList(_dataService.GetPedidosVenta(), "Id", "NumeroPedido", cobranza.IdPedidoVenta);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", cobranza);
            }
            return View(cobranza);
        }

        // GET: Cobranzas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var cobranza = _dataService.GetCobranzaById(id.Value);
            if (cobranza == null)
            {
                return NotFound();
            }

            ViewData["IdCliente"] = new SelectList(_dataService.GetClientes(), "Id", "NombreCompleto", cobranza.IdCliente);
            ViewData["IdPedidoVenta"] = new SelectList(_dataService.GetPedidosVenta(), "Id", "NumeroPedido", cobranza.IdPedidoVenta);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", cobranza);
            }
            return View(cobranza);
        }

        // POST: Cobranzas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,NumeroRecibo,IdCliente,IdPedidoVenta,FechaCobranza,Moneda,Monto,FormaPago,ReferenciaPago,Observaciones,Estado,Cobrador")] Cobranza cobranza)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateCobranza(cobranza);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Cobranza actualizada exitosamente" });
                }
                return RedirectToAction("Index");
            }

            ViewData["IdCliente"] = new SelectList(_dataService.GetClientes(), "Id", "NombreCompleto", cobranza.IdCliente);
            ViewData["IdPedidoVenta"] = new SelectList(_dataService.GetPedidosVenta(), "Id", "NumeroPedido", cobranza.IdPedidoVenta);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", cobranza);
            }
            return View(cobranza);
        }

        // GET: Cobranzas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var cobranza = _dataService.GetCobranzaById(id.Value);
            if (cobranza == null)
            {
                return NotFound();
            }

            // Load related entities for display
            if (cobranza.IdCliente.HasValue)
            {
                cobranza.Cliente = _dataService.GetClienteById(cobranza.IdCliente.Value);
            }
            if (cobranza.IdPedidoVenta.HasValue)
            {
                cobranza.PedidoVenta = _dataService.GetPedidoVentaById(cobranza.IdPedidoVenta.Value);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", cobranza);
            }
            return View(cobranza);
        }

        // POST: Cobranzas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteCobranza(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Cobranza eliminada exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleCobranzaEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}