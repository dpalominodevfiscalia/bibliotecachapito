using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;

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
            SetMenuItems();
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
            return View(profile);
        }

        // GET: Profiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Nombre,Apellido,FechaNacimiento")] Perfil profile)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddPerfil(profile);
                return RedirectToAction("Index");
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
            return View(profile);
        }

        // POST: Profiles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Nombre,Apellido,FechaNacimiento")] Perfil profile)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdatePerfil(profile);
                return RedirectToAction("Index");
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
            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _dataService.DeletePerfil(id);
            return RedirectToAction("Index");
        }
    }
}