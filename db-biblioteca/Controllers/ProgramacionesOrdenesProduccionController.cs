using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace BibliotecaDB.Controllers
{
    public class ProgramacionesOrdenesProduccionController : BaseController
    {
        public ProgramacionesOrdenesProduccionController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: ProgramacionesOrdenesProduccion
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateProgramacionPermission"] = HasOpcionActionPermission("ProgramacionesOrdenesProduccion", "Crear");
            HttpContext.Items["HasEditProgramacionPermission"] = HasOpcionActionPermission("ProgramacionesOrdenesProduccion", "Editar");
            HttpContext.Items["HasDetailsProgramacionPermission"] = HasOpcionActionPermission("ProgramacionesOrdenesProduccion", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteProgramacionPermission"] = HasOpcionActionPermission("ProgramacionesOrdenesProduccion", "Eliminar");
            HttpContext.Items["HasToggleProgramacionPermission"] = HasOpcionActionPermission("ProgramacionesOrdenesProduccion", "Desactivar");

            return View(_dataService.GetProgramacionesOrdenesProduccion());
        }

        // GET: ProgramacionesOrdenesProduccion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var programacionOrdenProduccion = _dataService.GetProgramacionOrdenProduccionById(id.Value);
            if (programacionOrdenProduccion == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", programacionOrdenProduccion);
            }
            return View(programacionOrdenProduccion);
        }

        // GET: ProgramacionesOrdenesProduccion/Create
        public ActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: ProgramacionesOrdenesProduccion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,CodigoOrden,Nombre,Descripcion,FechaProgramada,FechaInicio,FechaFin,Estado,Prioridad,Responsable,CreadoPor,FechaCreacion,ModificadoPor,FechaModificacion")] ProgramacionOrdenProduccion programacionOrdenProduccion)
        {
            if (ModelState.IsValid)
            {
                // Set default values
                programacionOrdenProduccion.Estado = string.IsNullOrEmpty(programacionOrdenProduccion.Estado) ? "Activo" : programacionOrdenProduccion.Estado;
                programacionOrdenProduccion.FechaCreacion = DateTime.Now;

                _dataService.AddProgramacionOrdenProduccion(programacionOrdenProduccion);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Programación de orden de producción creada exitosamente" });
                }
                return RedirectToAction("Index");
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", programacionOrdenProduccion);
            }
            return View(programacionOrdenProduccion);
        }

        // GET: ProgramacionesOrdenesProduccion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var programacionOrdenProduccion = _dataService.GetProgramacionOrdenProduccionById(id.Value);
            if (programacionOrdenProduccion == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", programacionOrdenProduccion);
            }
            return View(programacionOrdenProduccion);
        }

        // POST: ProgramacionesOrdenesProduccion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,CodigoOrden,Nombre,Descripcion,FechaProgramada,FechaInicio,FechaFin,Estado,Prioridad,Responsable,CreadoPor,FechaCreacion,ModificadoPor,FechaModificacion")] ProgramacionOrdenProduccion programacionOrdenProduccion)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateProgramacionOrdenProduccion(programacionOrdenProduccion);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Programación de orden de producción actualizada exitosamente" });
                }
                return RedirectToAction("Index");
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", programacionOrdenProduccion);
            }
            return View(programacionOrdenProduccion);
        }

        // GET: ProgramacionesOrdenesProduccion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var programacionOrdenProduccion = _dataService.GetProgramacionOrdenProduccionById(id.Value);
            if (programacionOrdenProduccion == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", programacionOrdenProduccion);
            }
            return View(programacionOrdenProduccion);
        }

        // POST: ProgramacionesOrdenesProduccion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteProgramacionOrdenProduccion(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Programación de orden de producción eliminada exitosamente" });
            }
            return RedirectToAction("Index");
        }

        // POST: ProgramacionesOrdenesProduccion/ToggleEstado/5
        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleProgramacionOrdenProduccionEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}