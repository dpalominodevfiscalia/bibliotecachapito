using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;

namespace BibliotecaDB.Controllers
{
    public class ImportacionesController : BaseController
    {
        public ImportacionesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Importaciones
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateImportacionPermission"] = HasOpcionActionPermission("Importaciones", "Crear");
            HttpContext.Items["HasEditImportacionPermission"] = HasOpcionActionPermission("Importaciones", "Editar");
            HttpContext.Items["HasDetailsImportacionPermission"] = HasOpcionActionPermission("Importaciones", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteImportacionPermission"] = HasOpcionActionPermission("Importaciones", "Eliminar");
            HttpContext.Items["HasToggleImportacionPermission"] = HasOpcionActionPermission("Importaciones", "Desactivar");

            return View(_dataService.GetImportaciones());
        }

        // GET: Importaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var importacion = _dataService.GetImportacionById(id.Value);
            if (importacion == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", importacion);
            }
            return View(importacion);
        }

        // GET: Importaciones/Create
        public ActionResult Create()
        {
            ViewBag.IdProveedor = new SelectList(_dataService.GetProveedores(), "Id", "Nombre");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial");
            }
            return View();
        }

        // POST: Importaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,NumeroImportacion,IdProveedor,FechaImportacion,PaisOrigen,PuertoEntrada,Incoterm,MedioTransporte,NumeroGuia,NumeroFactura,ValorFOB,ValorCIF,PorcentajeArancel,ValorArancel,PorcentajeIGV,ValorIGV,ValorTotal,Observaciones,Estado,EstadoAduana,FechaLlegada,FechaDespacho")] Importacion importacion)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddImportacion(importacion);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdProveedor = new SelectList(_dataService.GetProveedores(), "Id", "Nombre", importacion.IdProveedor);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", importacion);
            }
            return View(importacion);
        }

        // GET: Importaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var importacion = _dataService.GetImportacionById(id.Value);
            if (importacion == null)
            {
                return NotFound();
            }
            ViewBag.IdProveedor = new SelectList(_dataService.GetProveedores(), "Id", "Nombre", importacion.IdProveedor);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", importacion);
            }
            return View(importacion);
        }

        // POST: Importaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,NumeroImportacion,IdProveedor,FechaImportacion,PaisOrigen,PuertoEntrada,Incoterm,MedioTransporte,NumeroGuia,NumeroFactura,ValorFOB,ValorCIF,PorcentajeArancel,ValorArancel,PorcentajeIGV,ValorIGV,ValorTotal,Observaciones,Estado,EstadoAduana,FechaLlegada,FechaDespacho")] Importacion importacion)
        {
            if (ModelState.IsValid)
            {
                _dataService.UpdateImportacion(importacion);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdProveedor = new SelectList(_dataService.GetProveedores(), "Id", "Nombre", importacion.IdProveedor);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", importacion);
            }
            return View(importacion);
        }

        // GET: Importaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var importacion = _dataService.GetImportacionById(id.Value);
            if (importacion == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", importacion);
            }
            return View(importacion);
        }

        // POST: Importaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteImportacion(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleImportacionEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}