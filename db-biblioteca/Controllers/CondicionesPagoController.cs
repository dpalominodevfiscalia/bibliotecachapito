using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class CondicionesPagoController : BaseController
    {
        public CondicionesPagoController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: CondicionesPago
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateCondicionPagoPermission"] = HasOpcionActionPermission("CondicionesPago", "Crear");
            HttpContext.Items["HasEditCondicionPagoPermission"] = HasOpcionActionPermission("CondicionesPago", "Editar");
            HttpContext.Items["HasDetailsCondicionPagoPermission"] = HasOpcionActionPermission("CondicionesPago", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteCondicionPagoPermission"] = HasOpcionActionPermission("CondicionesPago", "Eliminar");
            HttpContext.Items["HasToggleCondicionPagoPermission"] = HasOpcionActionPermission("CondicionesPago", "Desactivar");

            return View(_dataService.GetCondicionesPago());
        }

        // GET: CondicionesPago/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var condicionPago = _dataService.GetCondicionPagoById(id.Value);
            if (condicionPago == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", condicionPago);
            }
            return View(condicionPago);
        }

        // GET: CondicionesPago/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: CondicionesPago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Descripcion,DiasCredito,Tipo,DescuentoPorProntoPago,FormaPago,Moneda,AplicaImpuestos,Observaciones")] CondicionPago condicionPago)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddCondicionPago(condicionPago);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Condición de pago creada exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", condicionPago);
            }
            return View(condicionPago);
        }

        // GET: CondicionesPago/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var condicionPago = _dataService.GetCondicionPagoById(id.Value);
            if (condicionPago == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", condicionPago);
            }
            return View(condicionPago);
        }

        // POST: CondicionesPago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Descripcion,DiasCredito,Tipo,DescuentoPorProntoPago,FormaPago,Moneda,AplicaImpuestos,Observaciones")] CondicionPago condicionPago)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateCondicionPago(condicionPago);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Condición de pago actualizada exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", condicionPago);
            }
            return View(condicionPago);
        }

        // GET: CondicionesPago/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var condicionPago = _dataService.GetCondicionPagoById(id.Value);
            if (condicionPago == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", condicionPago);
            }
            return View(condicionPago);
        }

        // POST: CondicionesPago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteCondicionPago(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Condición de pago eliminada exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleCondicionPagoEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}