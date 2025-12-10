using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Linq;

namespace BibliotecaDB.Controllers
{
    public class StocksController : BaseController
    {
        public StocksController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Stocks
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateStockPermission"] = HasOpcionActionPermission("Stocks", "Crear");
            HttpContext.Items["HasEditStockPermission"] = HasOpcionActionPermission("Stocks", "Editar");
            HttpContext.Items["HasDetailsStockPermission"] = HasOpcionActionPermission("Stocks", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteStockPermission"] = HasOpcionActionPermission("Stocks", "Eliminar");
            HttpContext.Items["HasToggleStockPermission"] = HasOpcionActionPermission("Stocks", "Desactivar");

            // Prepare dropdown data
            ViewBag.IdProducto = new SelectList(_dataService.GetProductos(), "Id", "Nombre");
            ViewBag.IdAlmacen = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre");

            var stocks = _dataService.GetStocks();
            foreach (var stock in stocks)
            {
                stock.Producto = _dataService.GetProductoById(stock.IdProducto);
                stock.Almacen = _dataService.GetAlmacenById(stock.IdAlmacen);
            }

            return View(stocks);
        }

        // GET: Stocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var stock = _dataService.GetStockById(id.Value);
            if (stock == null)
            {
                return NotFound();
            }

            // Load related entities
            stock.Producto = _dataService.GetProductoById(stock.IdProducto);
            stock.Almacen = _dataService.GetAlmacenById(stock.IdAlmacen);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", stock);
            }
            return View(stock);
        }

        // GET: Stocks/Create
        public ActionResult Create()
        {
            // Prepare dropdown data
            ViewBag.IdProducto = new SelectList(_dataService.GetProductos(), "Id", "Nombre");
            ViewBag.IdAlmacen = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Stocks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,IdProducto,IdAlmacen,Cantidad,FechaActualizacion,UsuarioActualizacion")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddStock(stock);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Stock creado exitosamente" });
                }
                return RedirectToAction("Index");
            }

            // Prepare dropdown data
            ViewBag.IdProducto = new SelectList(_dataService.GetProductos(), "Id", "Nombre", stock.IdProducto);
            ViewBag.IdAlmacen = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre", stock.IdAlmacen);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", stock);
            }
            return View(stock);
        }

        // GET: Stocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var stock = _dataService.GetStockById(id.Value);
            if (stock == null)
            {
                return NotFound();
            }

            // Prepare dropdown data
            ViewBag.IdProducto = new SelectList(_dataService.GetProductos(), "Id", "Nombre", stock.IdProducto);
            ViewBag.IdAlmacen = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre", stock.IdAlmacen);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", stock);
            }
            return View(stock);
        }

        // POST: Stocks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,IdProducto,IdAlmacen,Cantidad,FechaActualizacion,UsuarioActualizacion")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateStock(stock);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Stock actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }

            // Prepare dropdown data
            ViewBag.IdProducto = new SelectList(_dataService.GetProductos(), "Id", "Nombre", stock.IdProducto);
            ViewBag.IdAlmacen = new SelectList(_dataService.GetAlmacenes(), "Id", "Nombre", stock.IdAlmacen);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", stock);
            }
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var stock = _dataService.GetStockById(id.Value);
            if (stock == null)
            {
                return NotFound();
            }

            // Load related entities for display
            stock.Producto = _dataService.GetProductoById(stock.IdProducto);
            stock.Almacen = _dataService.GetAlmacenById(stock.IdAlmacen);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", stock);
            }
            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteStock(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Stock eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }
    }
}