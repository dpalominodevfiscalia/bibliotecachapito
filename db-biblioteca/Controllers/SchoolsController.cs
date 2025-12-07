using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class SchoolsController : BaseController
    {
        public SchoolsController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Schools
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateSchoolPermission"] = HasOpcionActionPermission("Schools", "Crear");
            HttpContext.Items["HasEditSchoolPermission"] = HasOpcionActionPermission("Schools", "Editar");
            HttpContext.Items["HasDetailsSchoolPermission"] = HasOpcionActionPermission("Schools", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteSchoolPermission"] = HasOpcionActionPermission("Schools", "Eliminar");
            HttpContext.Items["HasToggleSchoolPermission"] = HasOpcionActionPermission("Schools", "Desactivar");

            return View(_dataService.GetColegios());
        }

        // GET: Schools/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var school = _dataService.GetColegioById(id.Value);
            if (school == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", school);
            }
            return View(school);
        }

        // GET: Schools/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Schools/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Direccion,Telefono")] Colegio school)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddColegio(school);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Colegio creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", school);
            }
            return View(school);
        }

        // GET: Schools/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var school = _dataService.GetColegioById(id.Value);
            if (school == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", school);
            }
            return View(school);
        }

        // POST: Schools/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Direccion,Telefono")] Colegio school)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateColegio(school);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Colegio actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", school);
            }
            return View(school);
        }

        // GET: Schools/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var school = _dataService.GetColegioById(id.Value);
            if (school == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", school);
            }
            return View(school);
        }

        // POST: Schools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteColegio(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Colegio eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleColegioEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}