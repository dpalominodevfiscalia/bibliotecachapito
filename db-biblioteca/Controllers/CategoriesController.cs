using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Categories
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateCategoryPermission"] = HasOpcionActionPermission("Categories", "Crear");
            HttpContext.Items["HasEditCategoryPermission"] = HasOpcionActionPermission("Categories", "Editar");
            HttpContext.Items["HasDetailsCategoryPermission"] = HasOpcionActionPermission("Categories", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteCategoryPermission"] = HasOpcionActionPermission("Categories", "Eliminar");
            HttpContext.Items["HasToggleCategoryPermission"] = HasOpcionActionPermission("Categories", "Desactivar");

            return View(_dataService.GetCategorias());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var categoria = _dataService.GetCategoriaById(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", categoria);
            }
            return View(categoria);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Descripcion")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddCategoria(categoria);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Categoría creada exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", categoria);
            }
            return View(categoria);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var categoria = _dataService.GetCategoriaById(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", categoria);
            }
            return View(categoria);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Descripcion,Estado")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateCategoria(categoria);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Categoría actualizada exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", categoria);
            }
            return View(categoria);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var categoria = _dataService.GetCategoriaById(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", categoria);
            }
            return View(categoria);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteCategoria(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Categoría eliminada exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleCategoriaEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}