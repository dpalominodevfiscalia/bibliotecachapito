using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class RolesController : BaseController
    {
        public RolesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Roles
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateRolePermission"] = HasOpcionActionPermission("Roles", "Crear");
            HttpContext.Items["HasEditRolePermission"] = HasOpcionActionPermission("Roles", "Editar");
            HttpContext.Items["HasDetailsRolePermission"] = HasOpcionActionPermission("Roles", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteRolePermission"] = HasOpcionActionPermission("Roles", "Eliminar");
            HttpContext.Items["HasToggleRolePermission"] = HasOpcionActionPermission("Roles", "Desactivar");

            return View(_dataService.GetRoles());
        }

        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var role = _dataService.GetRolById(id.Value);
            if (role == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", role);
            }
            return View(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre")] Rol role)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddRol(role);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Rol creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", role);
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var role = _dataService.GetRolById(id.Value);
            if (role == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", role);
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre")] Rol role)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateRol(role);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Rol actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", role);
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var role = _dataService.GetRolById(id.Value);
            if (role == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", role);
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteRol(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Rol eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleRolEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}