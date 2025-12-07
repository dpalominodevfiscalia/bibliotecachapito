using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class RequestsController : BaseController
    {
        public RequestsController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Requests
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateRequestPermission"] = HasOpcionActionPermission("Requests", "Crear");
            HttpContext.Items["HasEditRequestPermission"] = HasOpcionActionPermission("Requests", "Editar");
            HttpContext.Items["HasDetailsRequestPermission"] = HasOpcionActionPermission("Requests", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteRequestPermission"] = HasOpcionActionPermission("Requests", "Eliminar");
            HttpContext.Items["HasToggleRequestPermission"] = HasOpcionActionPermission("Requests", "Desactivar");

            return View(_dataService.GetSolicitudes());
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var request = _dataService.GetSolicitudById(id.Value);
            if (request == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", request);
            }
            return View(request);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,IdUsuario,Descripcion,FechaSolicitud,Estado,IdSolicitud,AreaSolicitante,SolicitadoPor,Prioridad,EstadoSolicitud,ProductoServicio,CantidadSolicitada,Justificacion,FechaRequerida")] Solicitud request)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddSolicitud(request);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", request.IdUsuario);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", request);
            }
            return View(request);
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var request = _dataService.GetSolicitudById(id.Value);
            if (request == null)
            {
                return NotFound();
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", request.IdUsuario);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", request);
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,IdUsuario,Descripcion,FechaSolicitud,Estado,IdSolicitud,AreaSolicitante,SolicitadoPor,Prioridad,EstadoSolicitud,ProductoServicio,CantidadSolicitada,Justificacion,FechaRequerida")] Solicitud request)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateSolicitud(request);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(_dataService.GetUsuarios(), "Id", "NombreUsuario", request.IdUsuario);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", request);
            }
            return View(request);
        }

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var request = _dataService.GetSolicitudById(id.Value);
            if (request == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", request);
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteSolicitud(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleSolicitudEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}