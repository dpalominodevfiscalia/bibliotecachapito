using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;

namespace BibliotecaDB.Controllers
{
    public class SchoolsController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly DataService _dataService;

        public SchoolsController(IWebHostEnvironment env, DataService dataService)
        {
            _env = env;
            _dataService = dataService;
        }

        // GET: Schools
        public ActionResult Index()
        {
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
            return View(school);
        }

        // GET: Schools/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Schools/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Nombre,Direccion,Telefono")] Colegio school)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddColegio(school);
                return RedirectToAction("Index");
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
            return View(school);
        }

        // POST: Schools/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Nombre,Direccion,Telefono")] Colegio school)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateColegio(school);
                return RedirectToAction("Index");
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
            return View(school);
        }

        // POST: Schools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _dataService.DeleteColegio(id);
            return RedirectToAction("Index");
        }
    }
}