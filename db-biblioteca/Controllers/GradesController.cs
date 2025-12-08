using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Linq;

namespace BibliotecaDB.Controllers
{
    public class GradesController : BaseController
    {
        public GradesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Grades
        public ActionResult Index(int? nivelId = null)
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateGradePermission"] = HasOpcionActionPermission("Grades", "Crear");
            HttpContext.Items["HasEditGradePermission"] = HasOpcionActionPermission("Grades", "Editar");
            HttpContext.Items["HasDetailsGradePermission"] = HasOpcionActionPermission("Grades", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteGradePermission"] = HasOpcionActionPermission("Grades", "Eliminar");
            HttpContext.Items["HasToggleGradePermission"] = HasOpcionActionPermission("Grades", "Desactivar");

            // Create view model
            var viewModel = new GradesViewModel
            {
                Niveles = _dataService.GetNiveles(),
                Grados = _dataService.GetGrados()
            };

            // Filter grades by level if nivelId is provided
            if (nivelId.HasValue && nivelId.Value > 0)
            {
                viewModel.Grados = viewModel.Grados.Where(g => g.NivelId == nivelId.Value).ToList();
                viewModel.CurrentNivelId = nivelId.Value;

                var nivel = _dataService.GetNivelById(nivelId.Value);
                viewModel.CurrentNivelNombre = nivel?.Nombre ?? "Desconocido";
            }

            // Prepare dropdown data for Create modal
            ViewBag.NivelId = new SelectList(viewModel.Niveles, "Id", "Nombre");

            return View(viewModel);
        }

        // GET: Grades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var grade = _dataService.GetGradoById(id.Value);
            if (grade == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", grade);
            }
            return View(grade);
        }

        // GET: Grades/Create
        public ActionResult Create(int? nivelId = null)
        {
            // Prepare dropdown data for Nivel
            ViewBag.NivelId = new SelectList(_dataService.GetNiveles(), "Id", "Nombre");

            // Store the nivelId for auto-selection in the view
            if (nivelId.HasValue && nivelId.Value > 0)
            {
                ViewBag.SelectedNivelId = nivelId.Value;
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Grades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Descripcion,NivelId,Estado")] Grado grade)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddGrado(grade);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Grado creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", grade);
            }
            return View(grade);
        }

        // GET: Grades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var grade = _dataService.GetGradoById(id.Value);
            if (grade == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", grade);
            }
            return View(grade);
        }

        // POST: Grades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Descripcion,NivelId,Estado")] Grado grade)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateGrado(grade);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Grado actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", grade);
            }
            return View(grade);
        }

        // GET: Grades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var grade = _dataService.GetGradoById(id.Value);
            if (grade == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", grade);
            }
            return View(grade);
        }

        // POST: Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteGrado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Grado eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleGradoEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}