using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;

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
            SetMenuItems();
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
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(_dataService.GetRoles(), "Id", "Nombre");
            ViewBag.ProfileId = new SelectList(_dataService.GetPerfiles(), "Id", "Nombre");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,NombreUsuario,Contraseña,Correo,IdRol,IdPerfil")] Usuario user)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddUsuario(user);
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(_dataService.GetRoles(), "Id", "Nombre", user.IdRol);
            ViewBag.ProfileId = new SelectList(_dataService.GetPerfiles(), "Id", "Nombre", user.IdPerfil);
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
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,NombreUsuario,Contraseña,Correo,IdRol,IdPerfil")] Usuario user)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateUsuario(user);
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(_dataService.GetRoles(), "Id", "Nombre", user.IdRol);
            ViewBag.ProfileId = new SelectList(_dataService.GetPerfiles(), "Id", "Nombre", user.IdPerfil);
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
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _dataService.DeleteUsuario(id);
            return RedirectToAction("Index");
        }
    }
}