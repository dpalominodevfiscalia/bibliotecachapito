using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class ProductsController : BaseController
    {
        public ProductsController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Products
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateProductPermission"] = HasOpcionActionPermission("Products", "Crear");
            HttpContext.Items["HasEditProductPermission"] = HasOpcionActionPermission("Products", "Editar");
            HttpContext.Items["HasDetailsProductPermission"] = HasOpcionActionPermission("Products", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteProductPermission"] = HasOpcionActionPermission("Products", "Eliminar");
            HttpContext.Items["HasToggleProductPermission"] = HasOpcionActionPermission("Products", "Desactivar");

            return View(_dataService.GetProductos());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var producto = _dataService.GetProductoById(id.Value);
            if (producto == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", producto);
            }
            return View(producto);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.IdCategoria = new SelectList(_dataService.GetCategorias(), "Id", "Nombre");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,PrecioCompra,Stock,IdCategoria,ImagenUrl")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddProducto(producto);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Producto creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdCategoria = new SelectList(_dataService.GetCategorias(), "Id", "Nombre", producto.IdCategoria);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", producto);
            }
            return View(producto);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var producto = _dataService.GetProductoById(id.Value);
            if (producto == null)
            {
                return NotFound();
            }
            ViewBag.IdCategoria = new SelectList(_dataService.GetCategorias(), "Id", "Nombre", producto.IdCategoria);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", producto);
            }
            return View(producto);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Descripcion,Precio,PrecioCompra,Stock,IdCategoria,ImagenUrl,Estado")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateProducto(producto);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Producto actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdCategoria = new SelectList(_dataService.GetCategorias(), "Id", "Nombre", producto.IdCategoria);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", producto);
            }
            return View(producto);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var producto = _dataService.GetProductoById(id.Value);
            if (producto == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", producto);
            }
            return View(producto);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteProducto(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Producto eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleProductoEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}