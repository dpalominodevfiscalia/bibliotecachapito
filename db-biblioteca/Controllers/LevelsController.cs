using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Linq;

namespace BibliotecaDB.Controllers
{
    public class LevelsController : BaseController
    {
        public LevelsController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Levels
        public ActionResult Index(int? tipoClienteId = null)
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateLevelPermission"] = HasOpcionActionPermission("Levels", "Crear");
            HttpContext.Items["HasEditLevelPermission"] = HasOpcionActionPermission("Levels", "Editar");
            HttpContext.Items["HasDetailsLevelPermission"] = HasOpcionActionPermission("Levels", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteLevelPermission"] = HasOpcionActionPermission("Levels", "Eliminar");
            HttpContext.Items["HasToggleLevelPermission"] = HasOpcionActionPermission("Levels", "Desactivar");

            // Create view model
            var viewModel = new LevelsViewModel
            {
                TiposClientes = _dataService.GetTiposClientes()
            };

            // If filtering by client type, get levels for that client type
            if (tipoClienteId.HasValue && tipoClienteId.Value > 0)
            {
                var nivelesFiltrados = _dataService.GetNiveles()
                    .Where(n => n.TipoClienteId == tipoClienteId.Value)
                    .ToList();

                // Get client type name for display
                var tipoCliente = _dataService.GetTipoClienteById(tipoClienteId.Value);
                viewModel.Niveles = nivelesFiltrados;
                viewModel.TipoClienteId = tipoClienteId.Value;
                viewModel.TipoClienteNombre = tipoCliente?.Nombre ?? "Desconocido";
            }
            else
            {
                viewModel.Niveles = _dataService.GetNiveles();
            }

            return View(viewModel);
        }

        // GET: Levels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var level = _dataService.GetNivelById(id.Value);
            if (level == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", level);
            }
            return View(level);
        }

        // GET: Levels/Create
        public ActionResult Create()
        {
            // Prepare dropdown data for TipoCliente
            ViewBag.TipoClienteId = new SelectList(_dataService.GetTiposClientes(), "Id", "Nombre");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Levels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Descripcion,TipoClienteId,Estado")] Nivel level)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddNivel(level);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Nivel creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", level);
            }
            return View(level);
        }

        // GET: Levels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var level = _dataService.GetNivelById(id.Value);
            if (level == null)
            {
                return NotFound();
            }

            // Prepare dropdown data for TipoCliente
            ViewBag.TipoClienteId = new SelectList(_dataService.GetTiposClientes(), "Id", "Nombre");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", level);
            }
            return View(level);
        }

        // POST: Levels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Descripcion,TipoClienteId,Estado")] Nivel level)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateNivel(level);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Nivel actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", level);
            }
            return View(level);
        }

        // GET: Levels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var level = _dataService.GetNivelById(id.Value);
            if (level == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", level);
            }
            return View(level);
        }

        // POST: Levels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteNivel(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Nivel eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleNivelEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}