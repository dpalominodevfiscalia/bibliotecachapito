using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace BibliotecaDB.Controllers
{
    public class MovimientosAlmacenesController : BaseController
    {
        public MovimientosAlmacenesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: MovimientosAlmacenes
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateMovimientoAlmacenPermission"] = HasOpcionActionPermission("MovimientosAlmacenes", "Crear");
            HttpContext.Items["HasEditMovimientoAlmacenPermission"] = HasOpcionActionPermission("MovimientosAlmacenes", "Editar");
            HttpContext.Items["HasDetailsMovimientoAlmacenPermission"] = HasOpcionActionPermission("MovimientosAlmacenes", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteMovimientoAlmacenPermission"] = HasOpcionActionPermission("MovimientosAlmacenes", "Eliminar");
            HttpContext.Items["HasToggleMovimientoAlmacenPermission"] = HasOpcionActionPermission("MovimientosAlmacenes", "Desactivar");

            var movimientos = _dataService.GetMovimientosAlmacenes();
            foreach (var movimiento in movimientos)
            {
                movimiento.TipoMovimiento = _dataService.GetTipoMovimientoById(movimiento.IdTipoMovimiento);
                movimiento.AlmacenOrigen = _dataService.GetAlmacenById(movimiento.IdAlmacenOrigen);
                if (movimiento.IdAlmacenDestino.HasValue)
                {
                    movimiento.AlmacenDestino = _dataService.GetAlmacenById(movimiento.IdAlmacenDestino.Value);
                }
                movimiento.Producto = _dataService.GetProductoById(movimiento.IdProducto);
            }

            return View(movimientos);
        }

        // GET: MovimientosAlmacenes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var movimiento = _dataService.GetMovimientoAlmacenById(id.Value);
            if (movimiento == null)
            {
                return NotFound();
            }

            // Load related entities
            movimiento.TipoMovimiento = _dataService.GetTipoMovimientoById(movimiento.IdTipoMovimiento);
            movimiento.AlmacenOrigen = _dataService.GetAlmacenById(movimiento.IdAlmacenOrigen);
            if (movimiento.IdAlmacenDestino.HasValue)
            {
                movimiento.AlmacenDestino = _dataService.GetAlmacenById(movimiento.IdAlmacenDestino.Value);
            }
            movimiento.Producto = _dataService.GetProductoById(movimiento.IdProducto);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", movimiento);
            }
            return View(movimiento);
        }

        // GET: MovimientosAlmacenes/Create
        public ActionResult Create()
        {
            ViewData["IdTipoMovimiento"] = new SelectList(_dataService.GetTiposMovimientos(), "Id", "Nombre");
            ViewData["IdAlmacenOrigen"] = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre");
            ViewData["IdAlmacenDestino"] = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre");
            ViewData["IdProducto"] = new SelectList(_dataService.GetProductos(), "Id", "Nombre");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: MovimientosAlmacenes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,IdTipoMovimiento,IdAlmacenOrigen,IdAlmacenDestino,IdProducto,Cantidad,Observaciones,FechaMovimiento,Usuario,DocumentoReferencia,Estado")] MovimientoAlmacen movimiento)
        {
            if (ModelState.IsValid)
            {
                // Get movement type to determine if it's an entry or exit
                var tipoMovimiento = _dataService.GetTipoMovimientoById(movimiento.IdTipoMovimiento);

                // Update stock based on movement type
                await ActualizarStock(movimiento, tipoMovimiento.EsEntrada);

                _dataService.AddMovimientoAlmacen(movimiento);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Movimiento creado exitosamente" });
                }
                return RedirectToAction("Index");
            }

            ViewData["IdTipoMovimiento"] = new SelectList(_dataService.GetTiposMovimientos(), "Id", "Nombre", movimiento.IdTipoMovimiento);
            ViewData["IdAlmacenOrigen"] = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre", movimiento.IdAlmacenOrigen);
            ViewData["IdAlmacenDestino"] = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre", movimiento.IdAlmacenDestino);
            ViewData["IdProducto"] = new SelectList(_dataService.GetProductos(), "Id", "Nombre", movimiento.IdProducto);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", movimiento);
            }
            return View(movimiento);
        }

        // GET: MovimientosAlmacenes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var movimiento = _dataService.GetMovimientoAlmacenById(id.Value);
            if (movimiento == null)
            {
                return NotFound();
            }

            ViewData["IdTipoMovimiento"] = new SelectList(_dataService.GetTiposMovimientos(), "Id", "Nombre", movimiento.IdTipoMovimiento);
            ViewData["IdAlmacenOrigen"] = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre", movimiento.IdAlmacenOrigen);
            ViewData["IdAlmacenDestino"] = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre", movimiento.IdAlmacenDestino);
            ViewData["IdProducto"] = new SelectList(_dataService.GetProductos(), "Id", "Nombre", movimiento.IdProducto);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", movimiento);
            }
            return View(movimiento);
        }

        // POST: MovimientosAlmacenes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,IdTipoMovimiento,IdAlmacenOrigen,IdAlmacenDestino,IdProducto,Cantidad,Observaciones,FechaMovimiento,Usuario,DocumentoReferencia,Estado")] MovimientoAlmacen movimiento)
        {
            if (id != movimiento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get original movement to revert stock changes
                    var originalMovimiento = _dataService.GetMovimientoAlmacenById(id);
                    var originalTipo = _dataService.GetTipoMovimientoById(originalMovimiento.IdTipoMovimiento);

                    // Revert original stock changes
                    if (originalMovimiento != null)
                    {
                        await ActualizarStock(originalMovimiento, !originalTipo.EsEntrada); // Revert
                    }

                    // Get new movement type
                    var tipoMovimiento = _dataService.GetTipoMovimientoById(movimiento.IdTipoMovimiento);

                    // Apply new stock changes
                    await ActualizarStock(movimiento, tipoMovimiento.EsEntrada);

                    _dataService.UpdateMovimientoAlmacen(movimiento);

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, message = "Movimiento actualizado exitosamente" });
                    }
                }
                catch
                {
                    // Handle concurrency issues
                    if (!_dataService.GetMovimientosAlmacenes().Any(m => m.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            ViewData["IdTipoMovimiento"] = new SelectList(_dataService.GetTiposMovimientos(), "Id", "Nombre", movimiento.IdTipoMovimiento);
            ViewData["IdAlmacenOrigen"] = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre", movimiento.IdAlmacenOrigen);
            ViewData["IdAlmacenDestino"] = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre", movimiento.IdAlmacenDestino);
            ViewData["IdProducto"] = new SelectList(_dataService.GetProductos(), "Id", "Nombre", movimiento.IdProducto);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", movimiento);
            }
            return View(movimiento);
        }

        // GET: MovimientosAlmacenes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var movimiento = _dataService.GetMovimientoAlmacenById(id.Value);
            if (movimiento == null)
            {
                return NotFound();
            }

            // Load related entities for display
            movimiento.TipoMovimiento = _dataService.GetTipoMovimientoById(movimiento.IdTipoMovimiento);
            movimiento.AlmacenOrigen = _dataService.GetAlmacenById(movimiento.IdAlmacenOrigen);
            if (movimiento.IdAlmacenDestino.HasValue)
            {
                movimiento.AlmacenDestino = _dataService.GetAlmacenById(movimiento.IdAlmacenDestino.Value);
            }
            movimiento.Producto = _dataService.GetProductoById(movimiento.IdProducto);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", movimiento);
            }
            return View(movimiento);
        }

        // POST: MovimientosAlmacenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            // Get movement to revert stock changes
            var movimiento = _dataService.GetMovimientoAlmacenById(id);
            if (movimiento != null)
            {
                var tipoMovimiento = _dataService.GetTipoMovimientoById(movimiento.IdTipoMovimiento);

                // Revert stock changes when deleting movement
                await ActualizarStock(movimiento, !tipoMovimiento.EsEntrada); // Revert

                _dataService.DeleteMovimientoAlmacen(id);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Movimiento eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        // POST: MovimientosAlmacenes/ToggleEstado/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleMovimientoAlmacenEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        private async Task ActualizarStock(MovimientoAlmacen movimiento, bool esEntrada)
        {
            // Handle transfers between warehouses
            if (movimiento.IdAlmacenDestino.HasValue && movimiento.IdAlmacenDestino.Value != movimiento.IdAlmacenOrigen)
            {
                // Transfer: subtract from origin, add to destination
                await ActualizarStockAlmacen(movimiento.IdProducto, movimiento.IdAlmacenOrigen, -movimiento.Cantidad);
                await ActualizarStockAlmacen(movimiento.IdProducto, movimiento.IdAlmacenDestino.Value, movimiento.Cantidad);
            }
            else
            {
                // Regular movement: add or subtract based on movement type
                decimal cantidad = esEntrada ? movimiento.Cantidad : -movimiento.Cantidad;
                await ActualizarStockAlmacen(movimiento.IdProducto, movimiento.IdAlmacenOrigen, cantidad);
            }
        }

        private async Task ActualizarStockAlmacen(int idProducto, int idAlmacen, decimal cantidad)
        {
            var stock = _dataService.GetStocks().FirstOrDefault(s => s.IdProducto == idProducto && s.IdAlmacen == idAlmacen);

            if (stock == null)
            {
                // Create new stock record if it doesn't exist
                stock = new Stock
                {
                    IdProducto = idProducto,
                    IdAlmacen = idAlmacen,
                    Cantidad = cantidad,
                    FechaActualizacion = DateTime.Now,
                    UsuarioActualizacion = "System"
                };
                _dataService.AddStock(stock);
            }
            else
            {
                // Update existing stock
                stock.Cantidad += cantidad;
                stock.FechaActualizacion = DateTime.Now;
                stock.UsuarioActualizacion = "System";
                _dataService.UpdateStock(stock);
            }
        }
    }
}