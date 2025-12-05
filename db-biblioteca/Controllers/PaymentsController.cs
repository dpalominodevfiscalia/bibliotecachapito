using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;

namespace BibliotecaDB.Controllers
{
    public class PaymentsController : BaseController
    {
        public PaymentsController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Payments
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetMenuItems();
            return View(_dataService.GetPagos());
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var payment = _dataService.GetPagoById(id.Value);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            ViewBag.IdPedido = new SelectList(_dataService.GetPedidos(), "Id", "Id");
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,IdPedido,Monto,FechaPago,Metodo")] Pago payment)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddPago(payment);
                return RedirectToAction("Index");
            }
            ViewBag.IdPedido = new SelectList(_dataService.GetPedidos(), "Id", "Id", payment.IdPedido);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var payment = _dataService.GetPagoById(id.Value);
            if (payment == null)
            {
                return NotFound();
            }
            ViewBag.IdPedido = new SelectList(_dataService.GetPedidos(), "Id", "Id", payment.IdPedido);
            return View(payment);
        }

        // POST: Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,IdPedido,Monto,FechaPago,Metodo")] Pago payment)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdatePago(payment);
                return RedirectToAction("Index");
            }
            ViewBag.IdPedido = new SelectList(_dataService.GetPedidos(), "Id", "Id", payment.IdPedido);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var payment = _dataService.GetPagoById(id.Value);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _dataService.DeletePago(id);
            return RedirectToAction("Index");
        }
    }
}