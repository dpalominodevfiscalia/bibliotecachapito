using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class SalesController : BaseController
    {
        public SalesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Sales
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateSalePermission"] = HasOpcionActionPermission("Sales", "Crear");
            HttpContext.Items["HasEditSalePermission"] = HasOpcionActionPermission("Sales", "Editar");
            HttpContext.Items["HasDetailsSalePermission"] = HasOpcionActionPermission("Sales", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteSalePermission"] = HasOpcionActionPermission("Sales", "Eliminar");
            HttpContext.Items["HasToggleSalePermission"] = HasOpcionActionPermission("Sales", "Desactivar");

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
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", sale);
            }
            return View(sale);
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            var sale = new Venta();
            var currentUser = GetCurrentUser();

            // Auto-fill the user field with the current logged-in user
            if (currentUser != null)
            {
                sale.IdUsuario = currentUser.Id;
            }

            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", sale.IdUsuario);
            ViewBag.Productos = _dataService.GetProductos().Where(p => p.Estado == "Activo");
            ViewBag.Servicios = _dataService.GetServicios().Where(s => s.Estado == "Activo");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", sale);
            }
            return View(sale);
        }

        // POST: Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,IdUsuario,FechaVenta,Total,IdProducto,IdServicio,Cantidad,Tipo")] Venta sale)
        {
            // Custom validation: either IdProducto or IdServicio should be provided based on Tipo
            if (sale.Tipo == "Producto" && !sale.IdProducto.HasValue)
            {
                ModelState.AddModelError("IdProducto", "El producto es requerido para ventas de tipo Producto");
            }
            else if (sale.Tipo == "Servicio" && !sale.IdServicio.HasValue)
            {
                ModelState.AddModelError("IdServicio", "El servicio es requerido para ventas de tipo Servicio");
            }

            if (ModelState.IsValid)
            {
                // Auto-generate ID for new sales
                sale.Id = 0; // Set to 0 so the database will auto-generate the ID

                // Set current date/time if not provided
                if (sale.FechaVenta == default)
                {
                    sale.FechaVenta = DateTime.Now;
                }

                _dataService.AddVenta(sale);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Venta creada exitosamente" });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", sale.IdUsuario);
            ViewBag.Productos = _dataService.GetProductos().Where(p => p.Estado == "Activo");
            ViewBag.Servicios = _dataService.GetServicios().Where(s => s.Estado == "Activo");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", sale);
            }
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
            ViewBag.Productos = _dataService.GetProductos().Where(p => p.Estado == "Activo");
            ViewBag.Servicios = _dataService.GetServicios().Where(s => s.Estado == "Activo");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", sale);
            }
            return View(sale);
        }

        // POST: Sales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,IdUsuario,FechaVenta,Total,IdProducto,IdServicio,Cantidad,Tipo")] Venta sale)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateVenta(sale);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Venta actualizada exitosamente" });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", sale.IdUsuario);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", sale);
            }
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
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", sale);
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteVenta(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Venta eliminada exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleVentaEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}