using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BibliotecaDB.Controllers
{
    public class ModulosController : BaseController
    {
        public ModulosController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Modulos
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            //SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateModuloPermission"] = HasOpcionActionPermission("Modulos", "Crear");
            HttpContext.Items["HasEditModuloPermission"] = HasOpcionActionPermission("Modulos", "Editar");
            HttpContext.Items["HasDetailsModuloPermission"] = HasOpcionActionPermission("Modulos", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteModuloPermission"] = HasOpcionActionPermission("Modulos", "Eliminar");
            HttpContext.Items["HasToggleModuloPermission"] = HasOpcionActionPermission("Modulos", "Desactivar");

            return View(_dataService.GetModulos());
        }

        // GET: Modulos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var modulo = _dataService.GetModuloById(id.Value);
            if (modulo == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", modulo);
            }
            return View(modulo);
        }

        // GET: Modulos/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Modulos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Descripcion,Icono,Ruta,Orden")] Modulo modulo)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddModulo(modulo);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Módulo creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", modulo);
            }
            return View(modulo);
        }

        // GET: Modulos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var modulo = _dataService.GetModuloById(id.Value);
            if (modulo == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", modulo);
            }
            return View(modulo);
        }

        // POST: Modulos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Descripcion,Icono,Ruta,Orden")] Modulo modulo)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateModulo(modulo);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Módulo actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", modulo);
            }
            return View(modulo);
        }

        // GET: Modulos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var modulo = _dataService.GetModuloById(id.Value);
            if (modulo == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", modulo);
            }
            return View(modulo);
        }

        // POST: Modulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteModulo(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Módulo eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleModuloEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        // AJAX endpoint for module reordering
        [HttpPost]
        public ActionResult UpdateModuleOrder(List<int> moduleIds)
        {
            _dataService.ReorderModulos(moduleIds);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Orden de módulos actualizado exitosamente" });
            }
            return RedirectToAction("Index");
        }
    }
}