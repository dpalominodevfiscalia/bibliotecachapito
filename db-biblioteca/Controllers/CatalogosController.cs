using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace BibliotecaDB.Controllers
{
    public class CatalogosController : BaseController
    {
        public CatalogosController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Catalogos
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateCatalogoPermission"] = HasOpcionActionPermission("Catalogos", "Crear");
            HttpContext.Items["HasEditCatalogoPermission"] = HasOpcionActionPermission("Catalogos", "Editar");
            HttpContext.Items["HasDetailsCatalogoPermission"] = HasOpcionActionPermission("Catalogos", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteCatalogoPermission"] = HasOpcionActionPermission("Catalogos", "Eliminar");
            HttpContext.Items["HasToggleCatalogoPermission"] = HasOpcionActionPermission("Catalogos", "Desactivar");

            return View(_dataService.GetCatalogos());
        }

        // GET: Catalogos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var catalogo = _dataService.GetCatalogoById(id.Value);
            if (catalogo == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", catalogo);
            }
            return View(catalogo);
        }

        // GET: Catalogos/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Catalogos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,CatalogoId,GrupoName,GrupoId,Valor,Estado,Usuario,FechaRegistro")] Catalogo catalogo)
        {
            if (ModelState.IsValid)
            {
                // Set default values
                catalogo.Estado = string.IsNullOrEmpty(catalogo.Estado) ? "Activo" : catalogo.Estado;
                catalogo.FechaRegistro = DateTime.Now;

                _dataService.AddCatalogo(catalogo);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Catálogo creado exitosamente" });
                }
                return RedirectToAction("Index");
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", catalogo);
            }
            return View(catalogo);
        }

        // GET: Catalogos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var catalogo = _dataService.GetCatalogoById(id.Value);
            if (catalogo == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", catalogo);
            }
            return View(catalogo);
        }

        // POST: Catalogos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,CatalogoId,GrupoName,GrupoId,Valor,Estado,Usuario,FechaRegistro")] Catalogo catalogo)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateCatalogo(catalogo);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Catálogo actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", catalogo);
            }
            return View(catalogo);
        }

        // GET: Catalogos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var catalogo = _dataService.GetCatalogoById(id.Value);
            if (catalogo == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", catalogo);
            }
            return View(catalogo);
        }

        // POST: Catalogos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteCatalogo(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Catálogo eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleCatalogoEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}