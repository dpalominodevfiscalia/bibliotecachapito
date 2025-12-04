using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;

namespace BibliotecaDB.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly DataService _dataService;

        public ServicesController(IWebHostEnvironment env, DataService dataService)
        {
            _env = env;
            _dataService = dataService;
        }

        // GET: Services
        public ActionResult Index()
        {
            return View(_dataService.GetServicios());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var service = _dataService.GetServicioById(id.Value);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Nombre,Descripcion,Precio")] Servicio service)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddServicio(service);
                return RedirectToAction("Index");
            }
            return View(service);
        }

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var service = _dataService.GetServicioById(id.Value);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Nombre,Descripcion,Precio")] Servicio service)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateServicio(service);
                return RedirectToAction("Index");
            }
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var service = _dataService.GetServicioById(id.Value);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _dataService.DeleteServicio(id);
            return RedirectToAction("Index");
        }
    }
}