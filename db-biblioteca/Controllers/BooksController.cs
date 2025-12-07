using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class BooksController : BaseController
    {
        public BooksController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Books
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateBookPermission"] = HasOpcionActionPermission("Books", "Crear");
            HttpContext.Items["HasEditBookPermission"] = HasOpcionActionPermission("Books", "Editar");
            HttpContext.Items["HasDetailsBookPermission"] = HasOpcionActionPermission("Books", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteBookPermission"] = HasOpcionActionPermission("Books", "Eliminar");
            HttpContext.Items["HasToggleBookPermission"] = HasOpcionActionPermission("Books", "Desactivar");

            return View(_dataService.GetLibros());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var book = _dataService.GetLibroById(id.Value);
            if (book == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", book);
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Titulo,Autor,ISBN,Cantidad")] Libro book)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddLibro(book);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Libro creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", book);
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var book = _dataService.GetLibroById(id.Value);
            if (book == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", book);
            }
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Titulo,Autor,ISBN,Cantidad")] Libro book)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateLibro(book);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Libro actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", book);
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var book = _dataService.GetLibroById(id.Value);
            if (book == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", book);
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteLibro(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Libro eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleLibroEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}