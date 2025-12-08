using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Linq;

namespace BibliotecaDB.Controllers
{
    public class CompraDetallesController : BaseController
    {
        public CompraDetallesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: CompraDetalles
        public ActionResult Index(int idCompra)
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Get detalles for specific compra
            var detalles = _dataService.GetCompraDetalles().Where(cd => cd.IdCompra == idCompra).ToList();
            ViewBag.IdCompra = idCompra;

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateCompraDetallePermission"] = HasOpcionActionPermission("CompraDetalles", "Crear");
            HttpContext.Items["HasEditCompraDetallePermission"] = HasOpcionActionPermission("CompraDetalles", "Editar");
            HttpContext.Items["HasDetailsCompraDetallePermission"] = HasOpcionActionPermission("CompraDetalles", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteCompraDetallePermission"] = HasOpcionActionPermission("CompraDetalles", "Eliminar");
            HttpContext.Items["HasToggleCompraDetallePermission"] = HasOpcionActionPermission("CompraDetalles", "Desactivar");

            return View(detalles);
        }

        // GET: CompraDetalles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var compraDetalle = _dataService.GetCompraDetalleById(id.Value);
            if (compraDetalle == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", compraDetalle);
            }
            return View(compraDetalle);
        }

        // GET: CompraDetalles/Create
        public ActionResult Create(int idCompra)
        {
            ViewBag.IdCompra = idCompra;
            ViewBag.IdProducto = new SelectList(_dataService.GetProductos(), "Id", "Nombre");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: CompraDetalles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,IdCompra,IdProducto,Descripcion,UnidadMedida,CantidadSolicitada,CantidadRecibida,PrecioUnitario,TotalParcial")] CompraDetalle compraDetalle)
        {
            if (ModelState.IsValid)
            {
                // Calculate total parcial
                compraDetalle.TotalParcial = compraDetalle.CantidadSolicitada * compraDetalle.PrecioUnitario;

                _dataService.AddCompraDetalle(compraDetalle);

                // Update parent compra total
                var compra = _dataService.GetCompraById(compraDetalle.IdCompra);
                if (compra != null)
                {
                    compra.Total = _dataService.GetCompraDetalles()
                        .Where(cd => cd.IdCompra == compraDetalle.IdCompra)
                        .Sum(cd => cd.TotalParcial);
                    _dataService.UpdateCompra(compra);
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Detalle de compra creado exitosamente" });
                }
                return RedirectToAction("Index", new { idCompra = compraDetalle.IdCompra });
            }
            ViewBag.IdProducto = new SelectList(_dataService.GetProductos(), "Id", "Nombre", compraDetalle.IdProducto);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", compraDetalle);
            }
            return View(compraDetalle);
        }

        // GET: CompraDetalles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var compraDetalle = _dataService.GetCompraDetalleById(id.Value);
            if (compraDetalle == null)
            {
                return NotFound();
            }
            ViewBag.IdProducto = new SelectList(_dataService.GetProductos(), "Id", "Nombre", compraDetalle.IdProducto);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", compraDetalle);
            }
            return View(compraDetalle);
        }

        // POST: CompraDetalles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,IdCompra,IdProducto,Descripcion,UnidadMedida,CantidadSolicitada,CantidadRecibida,PrecioUnitario,TotalParcial")] CompraDetalle compraDetalle)
        {
            if (ModelState.IsValid)
            {
                // Calculate total parcial
                compraDetalle.TotalParcial = compraDetalle.CantidadSolicitada * compraDetalle.PrecioUnitario;

                _dataService.UpdateCompraDetalle(compraDetalle);

                // Update parent compra total
                var compra = _dataService.GetCompraById(compraDetalle.IdCompra);
                if (compra != null)
                {
                    compra.Total = _dataService.GetCompraDetalles()
                        .Where(cd => cd.IdCompra == compraDetalle.IdCompra)
                        .Sum(cd => cd.TotalParcial);
                    _dataService.UpdateCompra(compra);
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Detalle de compra actualizado exitosamente" });
                }
                return RedirectToAction("Index", new { idCompra = compraDetalle.IdCompra });
            }
            ViewBag.IdProducto = new SelectList(_dataService.GetProductos(), "Id", "Nombre", compraDetalle.IdProducto);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", compraDetalle);
            }
            return View(compraDetalle);
        }

        // GET: CompraDetalles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var compraDetalle = _dataService.GetCompraDetalleById(id.Value);
            if (compraDetalle == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", compraDetalle);
            }
            return View(compraDetalle);
        }

        // POST: CompraDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var compraDetalle = _dataService.GetCompraDetalleById(id);
            if (compraDetalle != null)
            {
                int idCompra = compraDetalle.IdCompra;

                _dataService.DeleteCompraDetalle(id);

                // Update parent compra total
                var compra = _dataService.GetCompraById(idCompra);
                if (compra != null)
                {
                    compra.Total = _dataService.GetCompraDetalles()
                        .Where(cd => cd.IdCompra == idCompra)
                        .Sum(cd => cd.TotalParcial);
                    _dataService.UpdateCompra(compra);
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Detalle de compra eliminado exitosamente" });
                }
                return RedirectToAction("Index", new { idCompra = idCompra });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleCompraDetalleEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}