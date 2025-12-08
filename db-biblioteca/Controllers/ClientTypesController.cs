using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class ClientTypesController : BaseController
    {
        public ClientTypesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: ClientTypes
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateClientTypePermission"] = HasOpcionActionPermission("ClientTypes", "Crear");
            HttpContext.Items["HasEditClientTypePermission"] = HasOpcionActionPermission("ClientTypes", "Editar");
            HttpContext.Items["HasDetailsClientTypePermission"] = HasOpcionActionPermission("ClientTypes", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteClientTypePermission"] = HasOpcionActionPermission("ClientTypes", "Eliminar");
            HttpContext.Items["HasToggleClientTypePermission"] = HasOpcionActionPermission("ClientTypes", "Desactivar");

            return View(_dataService.GetTiposClientes());
        }

        // GET: ClientTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var clientType = _dataService.GetTipoClienteById(id.Value);
            if (clientType == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", clientType);
            }
            return View(clientType);
        }

        // GET: ClientTypes/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: ClientTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Descripcion,Estado")] TipoCliente clientType)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddTipoCliente(clientType);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Tipo de cliente creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", clientType);
            }
            return View(clientType);
        }

        // GET: ClientTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var clientType = _dataService.GetTipoClienteById(id.Value);
            if (clientType == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", clientType);
            }
            return View(clientType);
        }

        // POST: ClientTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Descripcion,Estado")] TipoCliente clientType)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateTipoCliente(clientType);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Tipo de cliente actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", clientType);
            }
            return View(clientType);
        }

        // GET: ClientTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var clientType = _dataService.GetTipoClienteById(id.Value);
            if (clientType == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", clientType);
            }
            return View(clientType);
        }

        // POST: ClientTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteTipoCliente(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Tipo de cliente eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleTipoClienteEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}