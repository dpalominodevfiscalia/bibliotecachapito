using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace BibliotecaDB.Controllers
{
    public class RecetasController : BaseController
    {
        public RecetasController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Recetas
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateRecetaPermission"] = HasOpcionActionPermission("Recetas", "Crear");
            HttpContext.Items["HasEditRecetaPermission"] = HasOpcionActionPermission("Recetas", "Editar");
            HttpContext.Items["HasDetailsRecetaPermission"] = HasOpcionActionPermission("Recetas", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteRecetaPermission"] = HasOpcionActionPermission("Recetas", "Eliminar");
            HttpContext.Items["HasToggleRecetaPermission"] = HasOpcionActionPermission("Recetas", "Desactivar");

            return View(_dataService.GetRecetas());
        }

        // GET: Recetas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var receta = _dataService.GetRecetaById(id.Value);
            if (receta == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", receta);
            }
            return View(receta);
        }

        // GET: Recetas/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Recetas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,CodigoReceta,Nombre,Descripcion,Categoria,Tipo,TiempoPreparacion,Porciones,Dificultad,Estado,CreadoPor,FechaCreacion,ModificadoPor,FechaModificacion")] Receta receta)
        {
            if (ModelState.IsValid)
            {
                // Set default values
                receta.Estado = string.IsNullOrEmpty(receta.Estado) ? "Activo" : receta.Estado;
                receta.FechaCreacion = DateTime.Now;

                _dataService.AddReceta(receta);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Receta creada exitosamente" });
                }
                return RedirectToAction("Index");
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", receta);
            }
            return View(receta);
        }

        // GET: Recetas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var receta = _dataService.GetRecetaById(id.Value);
            if (receta == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", receta);
            }
            return View(receta);
        }

        // POST: Recetas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,CodigoReceta,Nombre,Descripcion,Categoria,Tipo,TiempoPreparacion,Porciones,Dificultad,Estado,CreadoPor,FechaCreacion,ModificadoPor,FechaModificacion")] Receta receta)
        {
            if (ModelState.IsValid)
            {
                // Set modification data
                receta.ModificadoPor = User.Identity.Name;
                receta.FechaModificacion = DateTime.Now;

                _dataService.UpdateReceta(receta);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Receta actualizada exitosamente" });
                }
                return RedirectToAction("Index");
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", receta);
            }
            return View(receta);
        }

        // GET: Recetas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var receta = _dataService.GetRecetaById(id.Value);
            if (receta == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", receta);
            }
            return View(receta);
        }

        // POST: Recetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteReceta(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Receta eliminada exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleRecetaEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}