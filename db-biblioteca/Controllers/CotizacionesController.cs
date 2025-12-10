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
    public class CotizacionesController : BaseController
    {
        public CotizacionesController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Cotizaciones
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Set permission flags based on Opcion configuration
            HttpContext.Items["HasCreateCotizacionPermission"] = HasOpcionActionPermission("Cotizaciones", "Crear");
            HttpContext.Items["HasEditCotizacionPermission"] = HasOpcionActionPermission("Cotizaciones", "Editar");
            HttpContext.Items["HasDetailsCotizacionPermission"] = HasOpcionActionPermission("Cotizaciones", "Editar"); // Details uses Edit permission
            HttpContext.Items["HasDeleteCotizacionPermission"] = HasOpcionActionPermission("Cotizaciones", "Eliminar");
            HttpContext.Items["HasToggleCotizacionPermission"] = HasOpcionActionPermission("Cotizaciones", "Desactivar");

            var cotizaciones = _dataService.GetCotizaciones();
            foreach (var cotizacion in cotizaciones)
            {
                if (cotizacion.IdCliente.HasValue)
                {
                    cotizacion.Cliente = _dataService.GetClienteById(cotizacion.IdCliente.Value);
                }
            }

            return View(cotizaciones);
        }

        // GET: Cotizaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var cotizacion = _dataService.GetCotizacionById(id.Value);
            if (cotizacion == null)
            {
                return NotFound();
            }

            // Load related client
            if (cotizacion.IdCliente.HasValue)
            {
                cotizacion.Cliente = _dataService.GetClienteById(cotizacion.IdCliente.Value);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", cotizacion);
            }
            return View(cotizacion);
        }

        // GET: Cotizaciones/Create
        public ActionResult Create()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            var clientes = _dataService.GetClientes();
            if (clientes == null || !clientes.Any())
            {
                // Handle case where no clients exist
                ModelState.AddModelError("", "No hay clientes disponibles. Por favor cree un cliente primero.");
                return View();
            }

            ViewData["IdCliente"] = new SelectList(clientes, "Id", "Nombre");

            // Set default values for new cotizacion
            var cotizacion = new Cotizacion
            {
                FechaCotizacion = DateTime.Now,
                FechaValidez = DateTime.Now.AddDays(30), // Default validity: 30 days
                Moneda = "PEN",
                Estado = "Pendiente",
                Subtotal = 0,
                Impuestos = 0,
                Total = 0
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", cotizacion);
            }
            return View(cotizacion);
        }

        // POST: Cotizaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,NumeroCotizacion,IdCliente,FechaCotizacion,FechaValidez,CondicionesPago,Moneda,Subtotal,Impuestos,Total,Observaciones,Estado,Vendedor")] Cotizacion cotizacion)
        {
            // Validate business logic
            if (cotizacion.FechaValidez <= cotizacion.FechaCotizacion)
            {
                ModelState.AddModelError("FechaValidez", "La fecha de validez debe ser posterior a la fecha de cotización");
            }

            // Auto-calculate total if not provided
            if (cotizacion.Subtotal > 0 && cotizacion.Impuestos > 0)
            {
                cotizacion.Total = cotizacion.Subtotal + cotizacion.Impuestos;
            }

            if (ModelState.IsValid)
            {
                // Set default values if not provided
                cotizacion.FechaCotizacion = cotizacion.FechaCotizacion == default ? DateTime.Now : cotizacion.FechaCotizacion;
                cotizacion.Moneda = string.IsNullOrEmpty(cotizacion.Moneda) ? "PEN" : cotizacion.Moneda;
                cotizacion.Estado = string.IsNullOrEmpty(cotizacion.Estado) ? "Pendiente" : cotizacion.Estado;

                _dataService.AddCotizacion(cotizacion);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Cotización creada exitosamente" });
                }
                return RedirectToAction("Index");
            }

            ViewData["IdCliente"] = new SelectList(_dataService.GetClientes(), "Id", "Nombre", cotizacion.IdCliente);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", cotizacion);
            }
            return View(cotizacion);
        }

        // GET: Cotizaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();
            if (id == null)
            {
                return BadRequest();
            }
            var cotizacion = _dataService.GetCotizacionById(id.Value);
            if (cotizacion == null)
            {
                return NotFound();
            }

            ViewData["IdCliente"] = new SelectList(_dataService.GetClientes(), "Id", "Nombre", cotizacion.IdCliente);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", cotizacion);
            }
            return View(cotizacion);
        }

        // POST: Cotizaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,NumeroCotizacion,IdCliente,FechaCotizacion,FechaValidez,CondicionesPago,Moneda,Subtotal,Impuestos,Total,Observaciones,Estado,Vendedor")] Cotizacion cotizacion)
        {
            // Validate business logic
            if (cotizacion.FechaValidez <= cotizacion.FechaCotizacion)
            {
                ModelState.AddModelError("FechaValidez", "La fecha de validez debe ser posterior a la fecha de cotización");
            }

            // Auto-calculate total if not provided
            if (cotizacion.Subtotal > 0 && cotizacion.Impuestos > 0)
            {
                cotizacion.Total = cotizacion.Subtotal + cotizacion.Impuestos;
            }

            if (ModelState.IsValid)
            {
                // Set default values if not provided
                cotizacion.Moneda = string.IsNullOrEmpty(cotizacion.Moneda) ? "PEN" : cotizacion.Moneda;
                cotizacion.Estado = string.IsNullOrEmpty(cotizacion.Estado) ? "Pendiente" : cotizacion.Estado;

                _dataService.UpdateCotizacion(cotizacion);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Cotización actualizada exitosamente" });
                }
                return RedirectToAction("Index");
            }

            ViewData["IdCliente"] = new SelectList(_dataService.GetClientes(), "Id", "NombreCompleto", cotizacion.IdCliente);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", cotizacion);
            }
            return View(cotizacion);
        }

        // GET: Cotizaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var cotizacion = _dataService.GetCotizacionById(id.Value);
            if (cotizacion == null)
            {
                return NotFound();
            }

            // Load related client for display
            if (cotizacion.IdCliente.HasValue)
            {
                cotizacion.Cliente = _dataService.GetClienteById(cotizacion.IdCliente.Value);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", cotizacion);
            }
            return View(cotizacion);
        }

        // POST: Cotizaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteCotizacion(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Cotización eliminada exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleCotizacionEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
    }
}