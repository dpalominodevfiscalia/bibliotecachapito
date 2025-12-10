using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class AlmacenesController : BaseController
    {
        public AlmacenesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Almacenes
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateAlmacenPermission"] = HasOpcionActionPermission("Almacenes", "Crear");
            HttpContext.Items["HasEditAlmacenPermission"] = HasOpcionActionPermission("Almacenes", "Editar");
            HttpContext.Items["HasDetailsAlmacenPermission"] = HasOpcionActionPermission("Almacenes", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteAlmacenPermission"] = HasOpcionActionPermission("Almacenes", "Eliminar");
            HttpContext.Items["HasToggleAlmacenPermission"] = HasOpcionActionPermission("Almacenes", "Desactivar");

            return View(_dataService.GetAlmacenes());
        }

        // GET: Almacenes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var almacen = _dataService.GetAlmacenById(id.Value);
            if (almacen == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", almacen);
            }
            return View(almacen);
        }

        // GET: Almacenes/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Almacenes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Descripcion,Ubicacion,Responsable,Estado,FechaCreacion")] Almacen almacen)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddAlmacen(almacen);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Almacén creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", almacen);
            }
            return View(almacen);
        }

        // GET: Almacenes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var almacen = _dataService.GetAlmacenById(id.Value);
            if (almacen == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", almacen);
            }
            return View(almacen);
        }

        // POST: Almacenes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Descripcion,Ubicacion,Responsable,Estado,FechaCreacion")] Almacen almacen)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateAlmacen(almacen);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Almacén actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", almacen);
            }
            return View(almacen);
        }

        // GET: Almacenes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var almacen = _dataService.GetAlmacenById(id.Value);
            if (almacen == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", almacen);
            }
            return View(almacen);
        }

        // POST: Almacenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteAlmacen(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Almacén eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleAlmacenEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}