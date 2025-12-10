using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class SupplierPaymentsController : BaseController
    {
        public SupplierPaymentsController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: SupplierPayments
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateSupplierPaymentPermission"] = HasOpcionActionPermission("SupplierPayments", "Crear");
            HttpContext.Items["HasEditSupplierPaymentPermission"] = HasOpcionActionPermission("SupplierPayments", "Editar");
            HttpContext.Items["HasDetailsSupplierPaymentPermission"] = HasOpcionActionPermission("SupplierPayments", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteSupplierPaymentPermission"] = HasOpcionActionPermission("SupplierPayments", "Eliminar");
            HttpContext.Items["HasToggleSupplierPaymentPermission"] = HasOpcionActionPermission("SupplierPayments", "Desactivar");

            return View(_dataService.GetPagosProveedores());
        }

        // GET: SupplierPayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var supplierPayment = _dataService.GetPagoProveedorById(id.Value);
            if (supplierPayment == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", supplierPayment);
            }
            return View(supplierPayment);
        }

        // GET: SupplierPayments/Create
        public ActionResult Create()
        {
            ViewBag.IdProveedor = new SelectList(_dataService.GetProveedores(), "Id", "Nombre");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: SupplierPayments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,IdProveedor,Monto,FechaPago,MetodoPago,Referencia,Observaciones,NumeroFactura,Moneda,TipoCambio")] PagoProveedor supplierPayment)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddPagoProveedor(supplierPayment);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Pago a proveedor creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdProveedor = new SelectList(_dataService.GetProveedores(), "Id", "Nombre", supplierPayment.IdProveedor);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", supplierPayment);
            }
            return View(supplierPayment);
        }

        // GET: SupplierPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var supplierPayment = _dataService.GetPagoProveedorById(id.Value);
            if (supplierPayment == null)
            {
                return NotFound();
            }
            ViewBag.IdProveedor = new SelectList(_dataService.GetProveedores(), "Id", "Nombre", supplierPayment.IdProveedor);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", supplierPayment);
            }
            return View(supplierPayment);
        }

        // POST: SupplierPayments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,IdProveedor,Monto,FechaPago,MetodoPago,Referencia,Observaciones,NumeroFactura,Moneda,TipoCambio")] PagoProveedor supplierPayment)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdatePagoProveedor(supplierPayment);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Pago a proveedor actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdProveedor = new SelectList(_dataService.GetProveedores(), "Id", "Nombre", supplierPayment.IdProveedor);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", supplierPayment);
            }
            return View(supplierPayment);
        }

        // GET: SupplierPayments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var supplierPayment = _dataService.GetPagoProveedorById(id.Value);
            if (supplierPayment == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", supplierPayment);
            }
            return View(supplierPayment);
        }

        // POST: SupplierPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeletePagoProveedor(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Pago a proveedor eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.TogglePagoProveedorEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}