using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class ComprasController : BaseController
    {
        public ComprasController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Compras
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateCompraPermission"] = HasOpcionActionPermission("Compras", "Crear");
            HttpContext.Items["HasEditCompraPermission"] = HasOpcionActionPermission("Compras", "Editar");
            HttpContext.Items["HasDetailsCompraPermission"] = HasOpcionActionPermission("Compras", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteCompraPermission"] = HasOpcionActionPermission("Compras", "Eliminar");
            HttpContext.Items["HasToggleCompraPermission"] = HasOpcionActionPermission("Compras", "Desactivar");

            return View(_dataService.GetCompras());
        }

        // GET: Compras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var compra = _dataService.GetCompraById(id.Value);
            if (compra == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", compra);
            }
            return View(compra);
        }

        // GET: Compras/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Compras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,IdUsuario,FechaCompra,Total")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddCompra(compra);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Compra creada exitosamente" });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", compra.IdUsuario);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", compra);
            }
            return View(compra);
        }

        // GET: Compras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var compra = _dataService.GetCompraById(id.Value);
            if (compra == null)
            {
                return NotFound();
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", compra.IdUsuario);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", compra);
            }
            return View(compra);
        }

        // POST: Compras/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,IdUsuario,FechaCompra,Total")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateCompra(compra);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Compra actualizada exitosamente" });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", compra.IdUsuario);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", compra);
            }
            return View(compra);
        }

        // GET: Compras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var compra = _dataService.GetCompraById(id.Value);
            if (compra == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", compra);
            }
            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteCompra(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Compra eliminada exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleCompraEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}