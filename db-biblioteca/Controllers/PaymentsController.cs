using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class PaymentsController : BaseController
    {
        public PaymentsController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Payments
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreatePaymentPermission"] = HasOpcionActionPermission("Payments", "Crear");
            HttpContext.Items["HasEditPaymentPermission"] = HasOpcionActionPermission("Payments", "Editar");
            HttpContext.Items["HasDetailsPaymentPermission"] = HasOpcionActionPermission("Payments", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeletePaymentPermission"] = HasOpcionActionPermission("Payments", "Eliminar");
            HttpContext.Items["HasTogglePaymentPermission"] = HasOpcionActionPermission("Payments", "Desactivar");

            return View(_dataService.GetPagos());
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var payment = _dataService.GetPagoById(id.Value);
            if (payment == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", payment);
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            ViewBag.IdCompra = new SelectList(_dataService.GetCompras(), "Id", "Id");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,IdCompra,Monto,FechaPago,Metodo")] Pago payment)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddPago(payment);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Pago creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdCompra = new SelectList(_dataService.GetCompras(), "Id", "Id", payment.IdCompra);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", payment);
            }
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var payment = _dataService.GetPagoById(id.Value);
            if (payment == null)
            {
                return NotFound();
            }
            ViewBag.IdCompra = new SelectList(_dataService.GetCompras(), "Id", "Id", payment.IdCompra);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", payment);
            }
            return View(payment);
        }

        // POST: Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,IdCompra,Monto,FechaPago,Metodo")] Pago payment)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdatePago(payment);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Pago actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdCompra = new SelectList(_dataService.GetCompras(), "Id", "Id", payment.IdCompra);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", payment);
            }
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var payment = _dataService.GetPagoById(id.Value);
            if (payment == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", payment);
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeletePago(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Pago eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.TogglePagoEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}