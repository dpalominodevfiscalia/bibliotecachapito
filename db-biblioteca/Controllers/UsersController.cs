using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Users
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateUserPermission"] = HasOpcionActionPermission("Users", "Crear");
            HttpContext.Items["HasEditUserPermission"] = HasOpcionActionPermission("Users", "Editar");
            HttpContext.Items["HasDetailsUserPermission"] = HasOpcionActionPermission("Users", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteUserPermission"] = HasOpcionActionPermission("Users", "Eliminar");
            HttpContext.Items["HasToggleUserPermission"] = HasOpcionActionPermission("Users", "Desactivar");

            return View(_dataService.GetUsuarios());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var user = _dataService.GetUsuarioById(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", user);
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(_dataService.GetRoles(), "Id", "Nombre");
            ViewBag.ProfileId = new SelectList(_dataService.GetPerfiles(), "Id", "Nombre");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,NombreUsuario,Contraseña,Correo,IdRol,IdPerfil")] Usuario user)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddUsuario(user);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Usuario creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(_dataService.GetRoles(), "Id", "Nombre", user.IdRol);
            ViewBag.ProfileId = new SelectList(_dataService.GetPerfiles(), "Id", "Nombre", user.IdPerfil);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", user);
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
                    if (id == null)
            {
                return BadRequest();
            }
            var user = _dataService.GetUsuarioById(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.RoleId = new SelectList(_dataService.GetRoles(), "Id", "Nombre", user.IdRol);
            ViewBag.ProfileId = new SelectList(_dataService.GetPerfiles(), "Id", "Nombre", user.IdPerfil);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", user);
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,NombreUsuario,Contraseña,Correo,IdRol,IdPerfil")] Usuario user)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateUsuario(user);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Usuario actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(_dataService.GetRoles(), "Id", "Nombre", user.IdRol);
            ViewBag.ProfileId = new SelectList(_dataService.GetPerfiles(), "Id", "Nombre", user.IdPerfil);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", user);
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var user = _dataService.GetUsuarioById(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", user);
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteUsuario(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Usuario eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleUsuarioEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}