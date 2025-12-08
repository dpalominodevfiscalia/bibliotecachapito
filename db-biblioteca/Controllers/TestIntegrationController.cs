using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Linq;

namespace BibliotecaDB.Controllers
{
    public class TestIntegrationController : BaseController
    {
        public TestIntegrationController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        public ActionResult TestCRUDIntegration()
        {
            try
            {
                // Test Proveedores CRUD
                var proveedor = new Proveedor
                {
                    Nombre = "Proveedor de Prueba",
                    RazonSocial = "Proveedor de Prueba S.A.",
                    RUC = "12345678901",
                    Direccion = "Calle Principal 123",
                    Telefono = "987654321",
                    Email = "prueba@proveedor.com",
                    Contacto = "Juan Pérez",
                    TelefonoContacto = "987654322",
                    EmailContacto = "juan@proveedor.com",
                    TipoProveedor = "Nacional",
                    CondicionesPago = "30 días",
                    Banco = "BCP",
                    NumeroCuenta = "123-4567890",
                    Moneda = "PEN",
                    Observaciones = "Proveedor de prueba para integración"
                };

                _dataService.AddProveedor(proveedor);
                var proveedorId = proveedor.Id;

                // Test Condiciones de Pago CRUD
                var condicionPago = new CondicionPago
                {
                    Nombre = "Condición de Prueba",
                    Descripcion = "Pago a 30 días con 5% descuento",
                    DiasCredito = 30,
                    Tipo = "Crédito",
                    DescuentoPorProntoPago = 5,
                    FormaPago = "Transferencia",
                    Moneda = "PEN",
                    AplicaImpuestos = "Si",
                    Observaciones = "Condición de prueba para integración"
                };

                _dataService.AddCondicionPago(condicionPago);
                var condicionPagoId = condicionPago.Id;

                // Test Compra with relationships
                var compra = new Compra
                {
                    IdUsuario = 1, // Assuming admin user exists
                    IdProveedor = proveedorId,
                    IdCondicionPago = condicionPagoId,
                    FechaCompra = System.DateTime.Now,
                    NumeroOrdenCompra = "TEST-001",
                    FechaEntrega = System.DateTime.Now.AddDays(7),
                    Observaciones = "Compra de prueba con integración completa"
                };

                _dataService.AddCompra(compra);
                var compraId = compra.Id;

                // Test CompraDetalle
                var compraDetalle = new CompraDetalle
                {
                    IdCompra = compraId,
                    IdProducto = 1, // Assuming a product exists
                    Descripcion = "Producto de prueba",
                    UnidadMedida = "Unidad",
                    CantidadSolicitada = 10,
                    CantidadRecibida = 10,
                    PrecioUnitario = 100,
                    TotalParcial = 1000 // Should be calculated automatically
                };

                _dataService.AddCompraDetalle(compraDetalle);

                // Verify relationships
                var savedCompra = _dataService.GetCompraById(compraId);
                var savedProveedor = _dataService.GetProveedorById(proveedorId);
                var savedCondicionPago = _dataService.GetCondicionPagoById(condicionPagoId);
                var savedDetalle = _dataService.GetCompraDetalleById(compraDetalle.Id);

                ViewBag.TestResults = new
                {
                    ProveedorCreated = savedProveedor != null,
                    CondicionPagoCreated = savedCondicionPago != null,
                    CompraCreated = savedCompra != null,
                    CompraDetalleCreated = savedDetalle != null,
                    CompraHasProveedor = savedCompra?.Proveedor != null,
                    CompraHasCondicionPago = savedCompra?.CondicionPago != null,
                    CompraHasDetalles = savedCompra?.Detalles != null && savedCompra.Detalles.Any(),
                    DetalleHasProducto = savedDetalle?.Producto != null,
                    DetalleHasCompra = savedDetalle?.Compra != null
                };

                return View();
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
    }
}