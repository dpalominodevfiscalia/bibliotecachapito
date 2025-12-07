using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class ServicesController : BaseController
    {
        public ServicesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Services
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateServicePermission"] = HasOpcionActionPermission("Services", "Crear");
            HttpContext.Items["HasEditServicePermission"] = HasOpcionActionPermission("Services", "Editar");
            HttpContext.Items["HasDetailsServicePermission"] = HasOpcionActionPermission("Services", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteServicePermission"] = HasOpcionActionPermission("Services", "Eliminar");
            HttpContext.Items["HasToggleServicePermission"] = HasOpcionActionPermission("Services", "Desactivar");

            return View(_dataService.GetServicios());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var service = _dataService.GetServicioById(id.Value);
            if (service == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", service);
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Descripcion,Precio")] Servicio service)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddServicio(service);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Servicio creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", service);
            }
            return View(service);
        }

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var service = _dataService.GetServicioById(id.Value);
            if (service == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", service);
            }
            return View(service);
        }

        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Descripcion,Precio")] Servicio service)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateServicio(service);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Servicio actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", service);
            }
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var service = _dataService.GetServicioById(id.Value);
            if (service == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", service);
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteServicio(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Servicio eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleServicioEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}