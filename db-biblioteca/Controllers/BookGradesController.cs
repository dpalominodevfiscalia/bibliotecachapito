using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Linq;

namespace BibliotecaDB.Controllers
{
    public class BookGradesController : BaseController
    {
        public BookGradesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: BookGrades
        public ActionResult Index(int? gradoId = null)
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateBookGradePermission"] = HasOpcionActionPermission("BookGrades", "Crear");
            HttpContext.Items["HasEditBookGradePermission"] = HasOpcionActionPermission("BookGrades", "Editar");
            HttpContext.Items["HasDetailsBookGradePermission"] = HasOpcionActionPermission("BookGrades", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteBookGradePermission"] = HasOpcionActionPermission("BookGrades", "Eliminar");
            HttpContext.Items["HasToggleBookGradePermission"] = HasOpcionActionPermission("BookGrades", "Desactivar");

            // Create view model
            var viewModel = new BookGradesViewModel
            {
                LibroGrados = _dataService.GetLibrosGrados(),
                Libros = _dataService.GetLibros(),
                Grados = _dataService.GetGrados()
            };

            // Filter by grade if gradoId is provided
            if (gradoId.HasValue && gradoId.Value > 0)
            {
                viewModel.LibroGrados = viewModel.LibroGrados.Where(lg => lg.GradoId == gradoId.Value).ToList();
                viewModel.CurrentGradoId = gradoId.Value;

                var grado = _dataService.GetGradoById(gradoId.Value);
                viewModel.CurrentGradoNombre = grado?.Nombre ?? "Desconocido";
            }

            return View(viewModel);
        }

        // GET: BookGrades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var bookGrade = _dataService.GetLibroGradoById(id.Value);
            if (bookGrade == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", bookGrade);
            }
            return View(bookGrade);
        }

        // GET: BookGrades/Create
        public ActionResult Create(int? gradoId = null)
        {
            // Prepare dropdown data
            ViewBag.LibroId = new SelectList(_dataService.GetLibros(), "Id", "Titulo");
            ViewBag.GradoId = new SelectList(_dataService.GetGrados(), "Id", "Nombre");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var model = new LibroGrado();
                if (gradoId.HasValue && gradoId.Value > 0)
                {
                    model.GradoId = gradoId.Value;
                }
                return PartialView("_CreatePartial", model);
            }
            return View();
        }

        // POST: BookGrades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,LibroId,GradoId,Estado")] LibroGrado bookGrade)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddLibroGrado(bookGrade);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Libro grado creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", bookGrade);
            }
            return View(bookGrade);
        }

        // GET: BookGrades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var bookGrade = _dataService.GetLibroGradoById(id.Value);
            if (bookGrade == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", bookGrade);
            }
            return View(bookGrade);
        }

        // POST: BookGrades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,LibroId,GradoId,Estado")] LibroGrado bookGrade)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateLibroGrado(bookGrade);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Libro grado actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", bookGrade);
            }
            return View(bookGrade);
        }

        // GET: BookGrades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var bookGrade = _dataService.GetLibroGradoById(id.Value);
            if (bookGrade == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", bookGrade);
            }
            return View(bookGrade);
        }

        // POST: BookGrades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteLibroGrado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Libro grado eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleLibroGradoEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}