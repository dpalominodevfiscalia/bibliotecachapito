using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;

namespace BibliotecaDB.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly DataService _dataService;

        public OrdersController(IWebHostEnvironment env, DataService dataService)
        {
            _env = env;
            _dataService = dataService;
        }

        // GET: Orders
        public ActionResult Index()
        {
            return View(_dataService.GetPedidos());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var order = _dataService.GetPedidoById(id.Value);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,IdUsuario,FechaPedido,Total")] Pedido order)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddPedido(order);
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", order.IdUsuario);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var order = _dataService.GetPedidoById(id.Value);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", order.IdUsuario);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,IdUsuario,FechaPedido,Total")] Pedido order)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdatePedido(order);
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", order.IdUsuario);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var order = _dataService.GetPedidoById(id.Value);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _dataService.DeletePedido(id);
            return RedirectToAction("Index");
        }
    }
}