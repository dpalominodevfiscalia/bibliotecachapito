using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class ClientsController : BaseController
    {
        public ClientsController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Clients
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateClientPermission"] = HasOpcionActionPermission("Clients", "Crear");
            HttpContext.Items["HasEditClientPermission"] = HasOpcionActionPermission("Clients", "Editar");
            HttpContext.Items["HasDetailsClientPermission"] = HasOpcionActionPermission("Clients", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteClientPermission"] = HasOpcionActionPermission("Clients", "Eliminar");
            HttpContext.Items["HasToggleClientPermission"] = HasOpcionActionPermission("Clients", "Desactivar");

            // Prepare dropdown data
            ViewBag.TipoClienteId = new SelectList(_dataService.GetTiposClientes(), "Id", "Nombre");

            return View(_dataService.GetClientes());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var client = _dataService.GetClienteById(id.Value);
            if (client == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", client);
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            // Prepare dropdown data for TipoCliente
            ViewBag.TipoClienteId = new SelectList(_dataService.GetTiposClientes(), "Id", "Nombre");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Apellido,Email,Telefono,Direccion,TipoDocumento,NumeroDocumento,FechaNacimiento,TipoClienteId,Estado")] Cliente client)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddCliente(client);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Cliente creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", client);
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var client = _dataService.GetClienteById(id.Value);
            if (client == null)
            {
                return NotFound();
            }

            // Prepare dropdown data for TipoCliente
            ViewBag.TipoClienteId = new SelectList(_dataService.GetTiposClientes(), "Id", "Nombre");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", client);
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Apellido,Email,Telefono,Direccion,TipoDocumento,NumeroDocumento,FechaNacimiento,TipoClienteId,Estado")] Cliente client)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateCliente(client);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Cliente actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", client);
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var client = _dataService.GetClienteById(id.Value);
            if (client == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", client);
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteCliente(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Cliente eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleClienteEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}