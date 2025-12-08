using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class ProveedoresController : BaseController
    {
        public ProveedoresController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Proveedores
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateProveedorPermission"] = HasOpcionActionPermission("Proveedores", "Crear");
            HttpContext.Items["HasEditProveedorPermission"] = HasOpcionActionPermission("Proveedores", "Editar");
            HttpContext.Items["HasDetailsProveedorPermission"] = HasOpcionActionPermission("Proveedores", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteProveedorPermission"] = HasOpcionActionPermission("Proveedores", "Eliminar");
            HttpContext.Items["HasToggleProveedorPermission"] = HasOpcionActionPermission("Proveedores", "Desactivar");

            return View(_dataService.GetProveedores());
        }

        // GET: Proveedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var proveedor = _dataService.GetProveedorById(id.Value);
            if (proveedor == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", proveedor);
            }
            return View(proveedor);
        }

        // GET: Proveedores/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Proveedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,RazonSocial,RUC,Direccion,Telefono,Email,Contacto,TelefonoContacto,EmailContacto,TipoProveedor,CondicionesPago,Banco,NumeroCuenta,Moneda,Observaciones")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddProveedor(proveedor);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Proveedor creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", proveedor);
            }
            return View(proveedor);
        }

        // GET: Proveedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var proveedor = _dataService.GetProveedorById(id.Value);
            if (proveedor == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", proveedor);
            }
            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,RazonSocial,RUC,Direccion,Telefono,Email,Contacto,TelefonoContacto,EmailContacto,TipoProveedor,CondicionesPago,Banco,NumeroCuenta,Moneda,Observaciones")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateProveedor(proveedor);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Proveedor actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", proveedor);
            }
            return View(proveedor);
        }

        // GET: Proveedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var proveedor = _dataService.GetProveedorById(id.Value);
            if (proveedor == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", proveedor);
            }
            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteProveedor(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Proveedor eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleProveedorEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}