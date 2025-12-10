using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class FacturacionesProveedoresController : BaseController
    {
        public FacturacionesProveedoresController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: FacturacionesProveedores
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateFacturacionPermission"] = HasOpcionActionPermission("FacturacionesProveedores", "Crear");
            HttpContext.Items["HasEditFacturacionPermission"] = HasOpcionActionPermission("FacturacionesProveedores", "Editar");
            HttpContext.Items["HasDetailsFacturacionPermission"] = HasOpcionActionPermission("FacturacionesProveedores", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteFacturacionPermission"] = HasOpcionActionPermission("FacturacionesProveedores", "Eliminar");
            HttpContext.Items["HasToggleFacturacionPermission"] = HasOpcionActionPermission("FacturacionesProveedores", "Desactivar");

            return View(_dataService.GetFacturacionesProveedores());
        }

        // GET: FacturacionesProveedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var facturacion = _dataService.GetFacturacionProveedorById(id.Value);
            if (facturacion == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", facturacion);
            }
            return View(facturacion);
        }

        // GET: FacturacionesProveedores/Create
        public ActionResult Create()
        {
            ViewBag.IdProveedor = new SelectList(_dataService.GetProveedores(), "Id", "Nombre");
            ViewBag.IdCompra = new SelectList(_dataService.GetCompras(), "Id", "NumeroOrdenCompra");
            ViewBag.IdImportacion = new SelectList(_dataService.GetImportaciones(), "Id", "NumeroImportacion");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: FacturacionesProveedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,NumeroFactura,IdProveedor,FechaFactura,FechaVencimiento,TipoDocumento,Subtotal,PorcentajeIGV,ValorIGV,Total,Moneda,TipoCambio,Observaciones,Estado,EstadoPago,FechaPago,MetodoPago,NumeroOperacion,IdCompra,IdImportacion")] FacturacionProveedor facturacion)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddFacturacionProveedor(facturacion);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdProveedor = new SelectList(_dataService.GetProveedores(), "Id", "Nombre", facturacion.IdProveedor);
            ViewBag.IdCompra = new SelectList(_dataService.GetCompras(), "Id", "NumeroOrdenCompra", facturacion.IdCompra);
            ViewBag.IdImportacion = new SelectList(_dataService.GetImportaciones(), "Id", "NumeroImportacion", facturacion.IdImportacion);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", facturacion);
            }
            return View(facturacion);
        }

        // GET: FacturacionesProveedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var facturacion = _dataService.GetFacturacionProveedorById(id.Value);
            if (facturacion == null)
            {
                return NotFound();
            }
            ViewBag.IdProveedor = new SelectList(_dataService.GetProveedores(), "Id", "Nombre", facturacion.IdProveedor);
            ViewBag.IdCompra = new SelectList(_dataService.GetCompras(), "Id", "NumeroOrdenCompra", facturacion.IdCompra);
            ViewBag.IdImportacion = new SelectList(_dataService.GetImportaciones(), "Id", "NumeroImportacion", facturacion.IdImportacion);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", facturacion);
            }
            return View(facturacion);
        }

        // POST: FacturacionesProveedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,NumeroFactura,IdProveedor,FechaFactura,FechaVencimiento,TipoDocumento,Subtotal,PorcentajeIGV,ValorIGV,Total,Moneda,TipoCambio,Observaciones,Estado,EstadoPago,FechaPago,MetodoPago,NumeroOperacion,IdCompra,IdImportacion")] FacturacionProveedor facturacion)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateFacturacionProveedor(facturacion);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdProveedor = new SelectList(_dataService.GetProveedores(), "Id", "Nombre", facturacion.IdProveedor);
            ViewBag.IdCompra = new SelectList(_dataService.GetCompras(), "Id", "NumeroOrdenCompra", facturacion.IdCompra);
            ViewBag.IdImportacion = new SelectList(_dataService.GetImportaciones(), "Id", "NumeroImportacion", facturacion.IdImportacion);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", facturacion);
            }
            return View(facturacion);
        }

        // GET: FacturacionesProveedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var facturacion = _dataService.GetFacturacionProveedorById(id.Value);
            if (facturacion == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", facturacion);
            }
            return View(facturacion);
        }

        // POST: FacturacionesProveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteFacturacionProveedor(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleFacturacionProveedorEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}