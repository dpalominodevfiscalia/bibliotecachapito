using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;

namespace BibliotecaDB.Controllers
{
    public class SalesController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly DataService _dataService;

        public SalesController(IWebHostEnvironment env, DataService dataService)
        {
            _env = env;
            _dataService = dataService;
        }

        // GET: Sales
        public ActionResult Index()
        {
            return View(_dataService.GetVentas());
        }

        // GET: Sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var sale = _dataService.GetVentaById(id.Value);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario");
            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,IdUsuario,FechaVenta,Total")] Venta sale)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddVenta(sale);
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", sale.IdUsuario);
            return View(sale);
        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var sale = _dataService.GetVentaById(id.Value);
            if (sale == null)
            {
                return NotFound();
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", sale.IdUsuario);
            return View(sale);
        }

        // POST: Sales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,IdUsuario,FechaVenta,Total")] Venta sale)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateVenta(sale);
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", sale.IdUsuario);
            return View(sale);
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var sale = _dataService.GetVentaById(id.Value);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _dataService.DeleteVenta(id);
            return RedirectToAction("Index");
        }
    }
}