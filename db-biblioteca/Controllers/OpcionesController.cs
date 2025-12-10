using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaDB.Controllers
{
    public class OpcionesController : BaseController
    {
        public OpcionesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Opciones
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;

            var opciones = _dataService.GetOpcionesWithModulos();
            return View(opciones);
        }

        // GET: Opciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var opcion = _dataService.GetOpcionById(id.Value);
            if (opcion == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", opcion);
            }
            return View(opcion);
        }

        // GET: Opciones/Create
        public ActionResult Create()
        {
            ViewBag.ModuloId = new SelectList(_dataService.GetModulos(), "Id", "Nombre");
            ViewBag.Controladores = new SelectList(_dataService.GetAvailableControllers());
            ViewBag.Acciones = new SelectList(_dataService.GetAvailableActions());

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Opciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Nombre,Descripcion,Controlador,Accion,Icono,Orden,Estado,ModuloId")] Opcion opcion)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddOpcion(opcion);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Opción creada exitosamente" });
                }
                return RedirectToAction("Index");
            }

            ViewBag.ModuloId = new SelectList(_dataService.GetModulos(), "Id", "Nombre", opcion.ModuloId);
            ViewBag.Controladores = new SelectList(_dataService.GetAvailableControllers());
            ViewBag.Acciones = new SelectList(_dataService.GetAvailableActions());

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", opcion);
            }
            return View(opcion);
        }

        // GET: Opciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var opcion = _dataService.GetOpcionById(id.Value);
            if (opcion == null)
            {
                return NotFound();
            }

            ViewBag.ModuloId = new SelectList(_dataService.GetModulos(), "Id", "Nombre", opcion.ModuloId);
            ViewBag.Controladores = new SelectList(_dataService.GetAvailableControllers());
            ViewBag.Acciones = new SelectList(_dataService.GetAvailableActions());

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", opcion);
            }
            return View(opcion);
        }

        // POST: Opciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nombre,Descripcion,Controlador,Accion,Icono,Orden,Estado,ModuloId")] Opcion opcion)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateOpcion(opcion);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Opción actualizada exitosamente" });
                }
                return RedirectToAction("Index");
            }

            ViewBag.ModuloId = new SelectList(_dataService.GetModulos(), "Id", "Nombre", opcion.ModuloId);
            ViewBag.Controladores = new SelectList(_dataService.GetAvailableControllers());
            ViewBag.Acciones = new SelectList(_dataService.GetAvailableActions());

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", opcion);
            }
            return View(opcion);
        }

        // GET: Opciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var opcion = _dataService.GetOpcionById(id.Value);
            if (opcion == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", opcion);
            }
            return View(opcion);
        }

        // POST: Opciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteOpcion(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Opción eliminada exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleOpcionEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}