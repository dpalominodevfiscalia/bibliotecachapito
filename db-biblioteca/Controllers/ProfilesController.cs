using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class ProfilesController : BaseController
    {
        public ProfilesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Profiles
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateProfilePermission"] = HasOpcionActionPermission("Profiles", "Crear");
            HttpContext.Items["HasEditProfilePermission"] = HasOpcionActionPermission("Profiles", "Editar");
            HttpContext.Items["HasDetailsProfilePermission"] = HasOpcionActionPermission("Profiles", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteProfilePermission"] = HasOpcionActionPermission("Profiles", "Eliminar");
            HttpContext.Items["HasToggleProfilePermission"] = HasOpcionActionPermission("Profiles", "Desactivar");

            return View(_dataService.GetPerfiles());
        }

        // GET: Profiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var profile = _dataService.GetPerfilById(id.Value);
            if (profile == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", profile);
            }
            return View(profile);
        }

        // GET: Profiles/Create
        public ActionResult Create()
        {
            ViewBag.OpcionItems = new SelectList(_dataService.GetOpcionItems(), "Id", "Title");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Profiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Apellido,FechaNacimiento,NumeroDocumento,Telefono,StartMenuId")] Perfil profile)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddPerfil(profile);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Perfil creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", profile);
            }
            return View(profile);
        }

        // GET: Profiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var profile = _dataService.GetPerfilById(id.Value);
            if (profile == null)
            {
                return NotFound();
            }
            ViewBag.OpcionItems = new SelectList(_dataService.GetOpcionItems(), "Id", "Title", profile.StartMenuId);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", profile);
            }
            return View(profile);
        }

        // POST: Profiles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Apellido,FechaNacimiento,NumeroDocumento,Telefono,StartMenuId")] Perfil profile)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdatePerfil(profile);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Perfil actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", profile);
            }
            return View(profile);
        }

        // GET: Profiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var profile = _dataService.GetPerfilById(id.Value);
            if (profile == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", profile);
            }
            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeletePerfil(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Perfil eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.TogglePerfilEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}