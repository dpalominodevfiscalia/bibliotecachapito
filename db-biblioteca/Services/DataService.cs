using BibliotecaDB.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BibliotecaDB.Services
{

    public class DataService
    {
        private readonly string _dataPath;
        private List<Usuario> _usuarios;
        private List<Libro> _libros;
        private List<Compra> _compras;
        private List<Pago> _pagos;
        private List<Perfil> _perfiles;
        private List<Solicitud> _solicitudes;
        private List<Rol> _roles;
        private List<Venta> _ventas;
        private List<Colegio> _colegios;
        private List<Servicio> _servicios;
        private List<MenuItem> _menuItems;
        private List<Accion> _acciones;
        private List<Producto> _productos;
        private List<Categoria> _categorias;
        private List<Cliente> _clientes;
        private List<TipoCliente> _tiposClientes;
        private List<Nivel> _niveles;
        private List<Grado> _grados;
        private List<LibroGrado> _librosGrados;
        private List<Proveedor> _proveedores;
        private List<CondicionPago> _condicionesPago;
        private List<CompraDetalle> _comprasDetalles;
        private List<Importacion> _importaciones;
        private List<ImportacionDetalle> _importacionesDetalles;
        private List<FacturacionProveedor> _facturacionesProveedores;
        private List<FacturacionDetalle> _facturacionesDetalles;
        private List<PagoProveedor> _pagosProveedores;
        private List<Modulo> _modulos;
        private List<Opcion> _opciones;
        private List<Almacen> _almacenes;
        private List<TipoMovimiento> _tiposMovimientos;
        private List<MovimientoAlmacen> _movimientosAlmacenes;
        private List<Stock> _stocks;
        private List<Cotizacion> _cotizaciones;
        private List<PedidoVenta> _pedidosVenta;
        private List<Cobranza> _cobranzas;
        private List<Catalogo> _catalogos;
        private List<ProgramacionOrdenProduccion> _programacionesOrdenesProduccion;

    public DataService(IWebHostEnvironment env)
    {
        _dataPath = Path.Combine(env.WebRootPath, "Data");
        LoadData();
    }

        private void LoadData()
        {
            _usuarios = LoadFromFile<Usuario>("users.json");
            _libros = LoadFromFile<Libro>("books.json");
            _compras = LoadFromFile<Compra>("orders.json");
            _pagos = LoadFromFile<Pago>("payments.json");
            _perfiles = LoadFromFile<Perfil>("profiles.json");
            _solicitudes = LoadFromFile<Solicitud>("requests.json");
            _roles = LoadFromFile<Rol>("roles.json");
            _ventas = LoadFromFile<Venta>("sales.json");
            _colegios = LoadFromFile<Colegio>("schools.json");
            _servicios = LoadFromFile<Servicio>("services.json");
            _menuItems = LoadFromFile<MenuItem>("opcion.json");
            _acciones = LoadFromFile<Accion>("actions.json");
            _productos = LoadFromFile<Producto>("products.json");
            _modulos = LoadFromFile<Modulo>("modulos.json");
            _opciones = LoadFromFile<Opcion>("opciones.json");
            _almacenes = LoadFromFile<Almacen>("almacenes.json");
            _tiposMovimientos = LoadFromFile<TipoMovimiento>("tiposMovimientos.json");
            _movimientosAlmacenes = LoadFromFile<MovimientoAlmacen>("movimientosAlmacenes.json");
            _stocks = LoadFromFile<Stock>("stocks.json");
            _cotizaciones = LoadFromFile<Cotizacion>("cotizaciones.json");
            _pedidosVenta = LoadFromFile<PedidoVenta>("pedidosVenta.json");
            _cobranzas = LoadFromFile<Cobranza>("cobranzas.json");
            _categorias = LoadFromFile<Categoria>("categories.json");
            _clientes = LoadFromFile<Cliente>("clients.json");
            _tiposClientes = LoadFromFile<TipoCliente>("clientTypes.json");
            _niveles = LoadFromFile<Nivel>("levels.json");
            _grados = LoadFromFile<Grado>("grades.json");
            _librosGrados = LoadFromFile<LibroGrado>("bookGrades.json");
            _proveedores = LoadFromFile<Proveedor>("proveedores.json");
            _condicionesPago = LoadFromFile<CondicionPago>("condicionesPago.json");
            _comprasDetalles = LoadFromFile<CompraDetalle>("comprasDetalles.json");
            _importaciones = LoadFromFile<Importacion>("importaciones.json");
            _importacionesDetalles = LoadFromFile<ImportacionDetalle>("importacionesDetalles.json");
            _facturacionesProveedores = LoadFromFile<FacturacionProveedor>("facturacionesProveedores.json");
            _facturacionesDetalles = LoadFromFile<FacturacionDetalle>("facturacionesDetalles.json");
            _pagosProveedores = LoadFromFile<PagoProveedor>("pagosProveedores.json");
            _catalogos = LoadFromFile<Catalogo>("catalogos.json");
            _recetas = LoadFromFile<Receta>("recetas.json");
            _programacionesOrdenesProduccion = LoadFromFile<ProgramacionOrdenProduccion>("programacionesOrdenesProduccion.json");
            PopulateUsuarioDetails();
            PopulateSolicitudUsuario();
            PopulateAccionRelations();
            PopulateVentaRelations();
            PopulateClientRelations();
        }

        private void PopulateUsuarioDetails()
        {
            foreach (var usuario in _usuarios)
            {
                usuario.Rol = _roles.FirstOrDefault(r => r.Id == usuario.IdRol);
                usuario.Perfil = _perfiles.FirstOrDefault(p => p.Id == usuario.IdPerfil);
            }
        }

        private void PopulateSolicitudUsuario()
        {
            foreach (var solicitud in _solicitudes)
            {
                solicitud.Usuario = _usuarios.FirstOrDefault(u => u.Id == solicitud.IdUsuario);
            }
        }

        private void PopulateAccionRelations()
        {
            // Method kept for compatibility but now empty since actions no longer have role/profile relationships
        }

        private void PopulateVentaRelations()
        {
            foreach (var venta in _ventas)
            {
                if (venta.IdProducto.HasValue)
                {
                    venta.Producto = _productos.FirstOrDefault(p => p.Id == venta.IdProducto.Value);
                }
                if (venta.IdServicio.HasValue)
                {
                    venta.Servicio = _servicios.FirstOrDefault(s => s.Id == venta.IdServicio.Value);
                }
                venta.Usuario = _usuarios.FirstOrDefault(u => u.Id == venta.IdUsuario);
            }
        }

        private void PopulateClientRelations()
        {
            // Populate client relationships
            foreach (var cliente in _clientes)
            {
                cliente.TipoCliente = _tiposClientes.FirstOrDefault(tc => tc.Id == cliente.TipoClienteId);
            }

            // Populate level relationships
            foreach (var nivel in _niveles)
            {
                nivel.TipoCliente = _tiposClientes.FirstOrDefault(tc => tc.Id == nivel.TipoClienteId);
            }

            // Populate grade relationships
            foreach (var grado in _grados)
            {
                grado.Nivel = _niveles.FirstOrDefault(n => n.Id == grado.NivelId);
            }

            // Populate book-grade relationships
            foreach (var libroGrado in _librosGrados)
            {
                libroGrado.Libro = _libros.FirstOrDefault(l => l.Id == libroGrado.LibroId);
                libroGrado.Grado = _grados.FirstOrDefault(g => g.Id == libroGrado.GradoId);
            }

            // Populate compra relationships
            foreach (var compra in _compras)
            {
                compra.Usuario = _usuarios.FirstOrDefault(u => u.Id == compra.IdUsuario);
                if (compra.IdProveedor.HasValue)
                {
                    compra.Proveedor = _proveedores.FirstOrDefault(p => p.Id == compra.IdProveedor.Value);
                }
                if (compra.IdCondicionPago.HasValue)
                {
                    compra.CondicionPago = _condicionesPago.FirstOrDefault(c => c.Id == compra.IdCondicionPago.Value);
                }

                // Populate compra detalles
                compra.Detalles = _comprasDetalles.Where(cd => cd.IdCompra == compra.Id).ToList();
                foreach (var detalle in compra.Detalles)
                {
                    detalle.Compra = compra;
                    detalle.Producto = _productos.FirstOrDefault(p => p.Id == detalle.IdProducto);
                }
   
                // Populate importacion relationships
               foreach (var importacion in _importaciones)
               {
                   importacion.Proveedor = _proveedores.FirstOrDefault(p => p.Id == importacion.IdProveedor);
                   importacion.Detalles = _importacionesDetalles.Where(id => id.IdImportacion == importacion.Id).ToList();
                   foreach (var detalle in importacion.Detalles)
                   {
                       detalle.Importacion = importacion;
                       detalle.Producto = _productos.FirstOrDefault(p => p.Id == detalle.IdProducto);
                   }
               }
   
               // Populate importacion detalle relationships
               foreach (var detalle in _importacionesDetalles)
               {
                   detalle.Producto = _productos.FirstOrDefault(p => p.Id == detalle.IdProducto);
               }
   
               // Populate facturacion relationships
               foreach (var facturacion in _facturacionesProveedores)
               {
                   facturacion.Proveedor = _proveedores.FirstOrDefault(p => p.Id == facturacion.IdProveedor);
                   facturacion.Compra = _compras.FirstOrDefault(c => c.Id == facturacion.IdCompra);
                   facturacion.Importacion = _importaciones.FirstOrDefault(i => i.Id == facturacion.IdImportacion);
                   facturacion.Detalles = _facturacionesDetalles.Where(fd => fd.IdFacturacion == facturacion.Id).ToList();
                   foreach (var detalle in facturacion.Detalles)
                   {
                       detalle.Facturacion = facturacion;
                       detalle.Producto = _productos.FirstOrDefault(p => p.Id == detalle.IdProducto);
                   }
               }
   
               // Populate facturacion detalle relationships
               foreach (var detalle in _facturacionesDetalles)
               {
                   detalle.Producto = _productos.FirstOrDefault(p => p.Id == detalle.IdProducto);
               }

               // Populate pago proveedor relationships
               foreach (var pago in _pagosProveedores)
               {
                   pago.Proveedor = _proveedores.FirstOrDefault(p => p.Id == pago.IdProveedor);
               }
           }

            // Populate compra detalle relationships
            foreach (var detalle in _comprasDetalles)
            {
                detalle.Producto = _productos.FirstOrDefault(p => p.Id == detalle.IdProducto);
            }
        }

        private List<T> LoadFromFile<T>(string fileName)
        {
            var filePath = Path.Combine(_dataPath, fileName);
            if (!File.Exists(filePath)) return new List<T>();
            var json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            return JsonSerializer.Deserialize<List<T>>(json, options) ?? new List<T>();
        }

        private void SaveToFile<T>(string fileName, List<T> data)
        {
            var filePath = Path.Combine(_dataPath, fileName);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
        }

        // Usuarios
        public List<Usuario> GetUsuarios() => _usuarios;
        public Usuario GetUsuarioById(int id) => _usuarios.FirstOrDefault(u => u.Id == id);
        public void AddUsuario(Usuario usuario)
        {
            usuario.Id = _usuarios.Max(u => u.Id) + 1;
            _usuarios.Add(usuario);
            SaveToFile("users.json", _usuarios);
        }
        public void UpdateUsuario(Usuario usuario)
        {
            var index = _usuarios.FindIndex(u => u.Id == usuario.Id);
            if (index != -1)
            {
                _usuarios[index] = usuario;
                SaveToFile("users.json", _usuarios);
            }
        }
        public void DeleteUsuario(int id)
        {
            _usuarios.RemoveAll(u => u.Id == id);
            SaveToFile("users.json", _usuarios);
        }

        // Libros
        public List<Libro> GetLibros() => _libros;
        public Libro GetLibroById(int id) => _libros.FirstOrDefault(l => l.Id == id);
        public void AddLibro(Libro libro)
        {
            libro.Id = _libros.Max(l => l.Id) + 1;
            _libros.Add(libro);
            SaveToFile("books.json", _libros);
        }
        public void UpdateLibro(Libro libro)
        {
            var index = _libros.FindIndex(l => l.Id == libro.Id);
            if (index != -1)
            {
                _libros[index] = libro;
                SaveToFile("books.json", _libros);
            }
        }
        public void DeleteLibro(int id)
        {
            _libros.RemoveAll(l => l.Id == id);
            SaveToFile("books.json", _libros);
        }

        // Compras
        public List<Compra> GetCompras() => _compras;
        public Compra GetCompraById(int id) => _compras.FirstOrDefault(o => o.Id == id);
        public void AddCompra(Compra compra)
        {
            compra.Id = _compras.Any() ? _compras.Max(o => o.Id) + 1 : 1;
            compra.Estado = "Activo";
            compra.FechaCompra = DateTime.Now;
            compra.NumeroOrdenCompra = compra.NumeroOrdenCompra ?? $"ORD-{compra.Id:D4}";

            // Calculate total from detalles if they exist
            if (compra.Detalles != null && compra.Detalles.Any())
            {
                compra.Total = compra.Detalles.Sum(d => d.TotalParcial);
            }

            _compras.Add(compra);
            SaveToFile("orders.json", _compras);
        }
        public void UpdateCompra(Compra compra)
        {
            var index = _compras.FindIndex(o => o.Id == compra.Id);
            if (index != -1)
            {
                // Calculate total from detalles if they exist
                if (compra.Detalles != null && compra.Detalles.Any())
                {
                    compra.Total = compra.Detalles.Sum(d => d.TotalParcial);
                }

                _compras[index] = compra;
                SaveToFile("orders.json", _compras);
            }
        }
        public void DeleteCompra(int id)
        {
            _compras.RemoveAll(o => o.Id == id);
            SaveToFile("orders.json", _compras);
        }

        // Proveedores
        public List<Proveedor> GetProveedores() => _proveedores;
        public Proveedor GetProveedorById(int id) => _proveedores.FirstOrDefault(p => p.Id == id);
        public void AddProveedor(Proveedor proveedor)
        {
            // Business logic validation
            if (string.IsNullOrWhiteSpace(proveedor.Nombre))
                throw new ArgumentException("El nombre del proveedor es obligatorio");

            if (string.IsNullOrWhiteSpace(proveedor.RUC) || proveedor.RUC.Length != 11)
                throw new ArgumentException("El RUC debe tener exactamente 11 caracteres");

            proveedor.Id = _proveedores.Any() ? _proveedores.Max(p => p.Id) + 1 : 1;
            proveedor.Estado = "Activo";
            _proveedores.Add(proveedor);
            SaveToFile("proveedores.json", _proveedores);
        }
        public void UpdateProveedor(Proveedor proveedor)
        {
            var index = _proveedores.FindIndex(p => p.Id == proveedor.Id);
            if (index != -1)
            {
                _proveedores[index] = proveedor;
                SaveToFile("proveedores.json", _proveedores);
            }
        }
        public void DeleteProveedor(int id)
        {
            _proveedores.RemoveAll(p => p.Id == id);
            SaveToFile("proveedores.json", _proveedores);
        }

        public void ToggleProveedorEstado(int id)
        {
            var proveedor = _proveedores.FirstOrDefault(p => p.Id == id);
            if (proveedor != null)
            {
                proveedor.Estado = proveedor.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("proveedores.json", _proveedores);
            }
        }

        // Condiciones de Pago
        public List<CondicionPago> GetCondicionesPago() => _condicionesPago;
        public CondicionPago GetCondicionPagoById(int id) => _condicionesPago.FirstOrDefault(c => c.Id == id);
        public void AddCondicionPago(CondicionPago condicionPago)
        {
            // Business logic validation
            if (string.IsNullOrWhiteSpace(condicionPago.Nombre))
                throw new ArgumentException("El nombre de la condición de pago es obligatorio");

            if (condicionPago.DiasCredito < 0 || condicionPago.DiasCredito > 365)
                throw new ArgumentException("Los días de crédito deben estar entre 0 y 365");

            if (condicionPago.DescuentoPorProntoPago < 0 || condicionPago.DescuentoPorProntoPago > 100)
                throw new ArgumentException("El descuento por pronto pago debe estar entre 0 y 100%");

            condicionPago.Id = _condicionesPago.Any() ? _condicionesPago.Max(c => c.Id) + 1 : 1;
            condicionPago.Estado = "Activo";
            _condicionesPago.Add(condicionPago);
            SaveToFile("condicionesPago.json", _condicionesPago);
        }
        public void UpdateCondicionPago(CondicionPago condicionPago)
        {
            var index = _condicionesPago.FindIndex(c => c.Id == condicionPago.Id);
            if (index != -1)
            {
                _condicionesPago[index] = condicionPago;
                SaveToFile("condicionesPago.json", _condicionesPago);
            }
        }
        public void DeleteCondicionPago(int id)
        {
            _condicionesPago.RemoveAll(c => c.Id == id);
            SaveToFile("condicionesPago.json", _condicionesPago);
        }

        public void ToggleCondicionPagoEstado(int id)
        {
            var condicionPago = _condicionesPago.FirstOrDefault(c => c.Id == id);
            if (condicionPago != null)
            {
                condicionPago.Estado = condicionPago.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("condicionesPago.json", _condicionesPago);
            }
        }

        // Almacenes
        public List<Almacen> GetAlmacenes() => _almacenes;
        public Almacen GetAlmacenById(int id) => _almacenes.FirstOrDefault(a => a.Id == id);
        public void AddAlmacen(Almacen almacen)
        {
            almacen.Id = _almacenes.Any() ? _almacenes.Max(a => a.Id) + 1 : 1;
            almacen.Estado = true;
            almacen.FechaCreacion = DateTime.Now;
            _almacenes.Add(almacen);
            SaveToFile("almacenes.json", _almacenes);
        }
        public void UpdateAlmacen(Almacen almacen)
        {
            var index = _almacenes.FindIndex(a => a.Id == almacen.Id);
            if (index != -1)
            {
                _almacenes[index] = almacen;
                SaveToFile("almacenes.json", _almacenes);
            }
        }
        public void DeleteAlmacen(int id)
        {
            _almacenes.RemoveAll(a => a.Id == id);
            SaveToFile("almacenes.json", _almacenes);
        }

        public void ToggleAlmacenEstado(int id)
        {
            var almacen = _almacenes.FirstOrDefault(a => a.Id == id);
            if (almacen != null)
            {
                almacen.Estado = !almacen.Estado;
                SaveToFile("almacenes.json", _almacenes);
            }
        }

        // TiposMovimientos
        public List<TipoMovimiento> GetTiposMovimientos() => _tiposMovimientos;
        public TipoMovimiento GetTipoMovimientoById(int id) => _tiposMovimientos.FirstOrDefault(t => t.Id == id);
        public void AddTipoMovimiento(TipoMovimiento tipoMovimiento)
        {
            tipoMovimiento.Id = _tiposMovimientos.Any() ? _tiposMovimientos.Max(t => t.Id) + 1 : 1;
            tipoMovimiento.Estado = true;
            tipoMovimiento.FechaCreacion = DateTime.Now;
            _tiposMovimientos.Add(tipoMovimiento);
            SaveToFile("tiposMovimientos.json", _tiposMovimientos);
        }
        public void UpdateTipoMovimiento(TipoMovimiento tipoMovimiento)
        {
            var index = _tiposMovimientos.FindIndex(t => t.Id == tipoMovimiento.Id);
            if (index != -1)
            {
                _tiposMovimientos[index] = tipoMovimiento;
                SaveToFile("tiposMovimientos.json", _tiposMovimientos);
            }
        }
        public void DeleteTipoMovimiento(int id)
        {
            _tiposMovimientos.RemoveAll(t => t.Id == id);
            SaveToFile("tiposMovimientos.json", _tiposMovimientos);
        }

        public void ToggleTipoMovimientoEstado(int id)
        {
            var tipoMovimiento = _tiposMovimientos.FirstOrDefault(t => t.Id == id);
            if (tipoMovimiento != null)
            {
                tipoMovimiento.Estado = !tipoMovimiento.Estado;
                SaveToFile("tiposMovimientos.json", _tiposMovimientos);
            }
        }

        // MovimientosAlmacenes
        public List<MovimientoAlmacen> GetMovimientosAlmacenes() => _movimientosAlmacenes;
        public MovimientoAlmacen GetMovimientoAlmacenById(int id) => _movimientosAlmacenes.FirstOrDefault(m => m.Id == id);
        public void AddMovimientoAlmacen(MovimientoAlmacen movimiento)
        {
            movimiento.Id = _movimientosAlmacenes.Any() ? _movimientosAlmacenes.Max(m => m.Id) + 1 : 1;
            movimiento.Estado = true;
            movimiento.FechaMovimiento = DateTime.Now;
            _movimientosAlmacenes.Add(movimiento);
            SaveToFile("movimientosAlmacenes.json", _movimientosAlmacenes);
        }
        public void UpdateMovimientoAlmacen(MovimientoAlmacen movimiento)
        {
            var index = _movimientosAlmacenes.FindIndex(m => m.Id == movimiento.Id);
            if (index != -1)
            {
                _movimientosAlmacenes[index] = movimiento;
                SaveToFile("movimientosAlmacenes.json", _movimientosAlmacenes);
            }
        }
        public void DeleteMovimientoAlmacen(int id)
        {
            _movimientosAlmacenes.RemoveAll(m => m.Id == id);
            SaveToFile("movimientosAlmacenes.json", _movimientosAlmacenes);
        }

        public void ToggleMovimientoAlmacenEstado(int id)
        {
            var movimiento = _movimientosAlmacenes.FirstOrDefault(m => m.Id == id);
            if (movimiento != null)
            {
                movimiento.Estado = !movimiento.Estado;
                SaveToFile("movimientosAlmacenes.json", _movimientosAlmacenes);
            }
        }

        // Stocks
        public List<Stock> GetStocks() => _stocks;
        public Stock GetStockById(int id) => _stocks.FirstOrDefault(s => s.Id == id);
        public void AddStock(Stock stock)
        {
            stock.Id = _stocks.Any() ? _stocks.Max(s => s.Id) + 1 : 1;
            stock.FechaActualizacion = DateTime.Now;
            _stocks.Add(stock);
            SaveToFile("stocks.json", _stocks);
        }
        public void UpdateStock(Stock stock)
        {
            var index = _stocks.FindIndex(s => s.Id == stock.Id);
            if (index != -1)
            {
                _stocks[index] = stock;
                SaveToFile("stocks.json", _stocks);
            }
        }
        public void DeleteStock(int id)
        {
            _stocks.RemoveAll(s => s.Id == id);
            SaveToFile("stocks.json", _stocks);
        }

        // Cotizaciones
        public List<Cotizacion> GetCotizaciones() => _cotizaciones;
        public Cotizacion GetCotizacionById(int id) => _cotizaciones.FirstOrDefault(c => c.Id == id);
        public void AddCotizacion(Cotizacion cotizacion)
        {
            cotizacion.Id = _cotizaciones.Any() ? _cotizaciones.Max(c => c.Id) + 1 : 1;
            cotizacion.FechaCotizacion = DateTime.Now;
            cotizacion.Estado = "Pendiente";
            _cotizaciones.Add(cotizacion);
            SaveToFile("cotizaciones.json", _cotizaciones);
        }
        public void UpdateCotizacion(Cotizacion cotizacion)
        {
            var index = _cotizaciones.FindIndex(c => c.Id == cotizacion.Id);
            if (index != -1)
            {
                _cotizaciones[index] = cotizacion;
                SaveToFile("cotizaciones.json", _cotizaciones);
            }
        }
        public void DeleteCotizacion(int id)
        {
            _cotizaciones.RemoveAll(c => c.Id == id);
            SaveToFile("cotizaciones.json", _cotizaciones);
        }

        public void ToggleCotizacionEstado(int id)
        {
            var cotizacion = _cotizaciones.FirstOrDefault(c => c.Id == id);
            if (cotizacion != null)
            {
                cotizacion.Estado = cotizacion.Estado == "Pendiente" ? "Aprobada" : "Pendiente";
                SaveToFile("cotizaciones.json", _cotizaciones);
            }
        }

        // PedidosVenta
        public List<PedidoVenta> GetPedidosVenta() => _pedidosVenta;
        public PedidoVenta GetPedidoVentaById(int id) => _pedidosVenta.FirstOrDefault(p => p.Id == id);
        public void AddPedidoVenta(PedidoVenta pedido)
        {
            pedido.Id = _pedidosVenta.Any() ? _pedidosVenta.Max(p => p.Id) + 1 : 1;
            pedido.FechaPedido = DateTime.Now;
            pedido.Estado = "Pendiente";
            _pedidosVenta.Add(pedido);
            SaveToFile("pedidosVenta.json", _pedidosVenta);
        }
        public void UpdatePedidoVenta(PedidoVenta pedido)
        {
            var index = _pedidosVenta.FindIndex(p => p.Id == pedido.Id);
            if (index != -1)
            {
                _pedidosVenta[index] = pedido;
                SaveToFile("pedidosVenta.json", _pedidosVenta);
            }
        }
        public void DeletePedidoVenta(int id)
        {
            _pedidosVenta.RemoveAll(p => p.Id == id);
            SaveToFile("pedidosVenta.json", _pedidosVenta);
        }

        public void TogglePedidoVentaEstado(int id)
        {
            var pedido = _pedidosVenta.FirstOrDefault(p => p.Id == id);
            if (pedido != null)
            {
                // Cycle through states: Pendiente -> Aprobado -> Entregado -> Pendiente
                switch (pedido.Estado)
                {
                    case "Pendiente":
                        pedido.Estado = "Aprobado";
                        break;
                    case "Aprobado":
                        pedido.Estado = "Entregado";
                        break;
                    case "Entregado":
                        pedido.Estado = "Pendiente";
                        break;
                    default:
                        pedido.Estado = "Pendiente";
                        break;
                }
                SaveToFile("pedidosVenta.json", _pedidosVenta);
            }
        }

        // Cobranzas
        public List<Cobranza> GetCobranzas() => _cobranzas;
        public Cobranza GetCobranzaById(int id) => _cobranzas.FirstOrDefault(c => c.Id == id);
        public void AddCobranza(Cobranza cobranza)
        {
            cobranza.Id = _cobranzas.Any() ? _cobranzas.Max(c => c.Id) + 1 : 1;
            cobranza.FechaCobranza = DateTime.Now;
            cobranza.Estado = "Pendiente";
            _cobranzas.Add(cobranza);
            SaveToFile("cobranzas.json", _cobranzas);
        }
        public void UpdateCobranza(Cobranza cobranza)
        {
            var index = _cobranzas.FindIndex(c => c.Id == cobranza.Id);
            if (index != -1)
            {
                _cobranzas[index] = cobranza;
                SaveToFile("cobranzas.json", _cobranzas);
            }
        }
        public void DeleteCobranza(int id)
        {
            _cobranzas.RemoveAll(c => c.Id == id);
            SaveToFile("cobranzas.json", _cobranzas);
        }

        public void ToggleCobranzaEstado(int id)
        {
            var cobranza = _cobranzas.FirstOrDefault(c => c.Id == id);
            if (cobranza != null)
            {
                // Cycle through states: Pendiente -> Cobrado -> Anulado -> Pendiente
                switch (cobranza.Estado)
                {
                    case "Pendiente":
                        cobranza.Estado = "Cobrado";
                        break;
                    case "Cobrado":
                        cobranza.Estado = "Anulado";
                        break;
                    case "Anulado":
                        cobranza.Estado = "Pendiente";
                        break;
                    default:
                        cobranza.Estado = "Pendiente";
                        break;
                }
                SaveToFile("cobranzas.json", _cobranzas);
            }
        }

        // Compra Detalles
        public List<CompraDetalle> GetCompraDetalles() => _comprasDetalles;
        public CompraDetalle GetCompraDetalleById(int id) => _comprasDetalles.FirstOrDefault(cd => cd.Id == id);
        public void AddCompraDetalle(CompraDetalle compraDetalle)
        {
            // Business logic validation
            if (compraDetalle.CantidadSolicitada <= 0)
                throw new ArgumentException("La cantidad solicitada debe ser mayor que 0");

            if (compraDetalle.PrecioUnitario <= 0)
                throw new ArgumentException("El precio unitario debe ser mayor que 0");

            // Calculate total parcial
            compraDetalle.TotalParcial = compraDetalle.CantidadSolicitada * compraDetalle.PrecioUnitario;

            compraDetalle.Id = _comprasDetalles.Any() ? _comprasDetalles.Max(cd => cd.Id) + 1 : 1;
            compraDetalle.Estado = "Activo";
            _comprasDetalles.Add(compraDetalle);
            SaveToFile("comprasDetalles.json", _comprasDetalles);
        }
        public void UpdateCompraDetalle(CompraDetalle compraDetalle)
        {
            var index = _comprasDetalles.FindIndex(cd => cd.Id == compraDetalle.Id);
            if (index != -1)
            {
                _comprasDetalles[index] = compraDetalle;
                SaveToFile("comprasDetalles.json", _comprasDetalles);
            }
        }
        public void DeleteCompraDetalle(int id)
        {
            _comprasDetalles.RemoveAll(cd => cd.Id == id);
            SaveToFile("comprasDetalles.json", _comprasDetalles);
        }

        public void ToggleCompraDetalleEstado(int id)
        {
            var compraDetalle = _comprasDetalles.FirstOrDefault(cd => cd.Id == id);
            if (compraDetalle != null)
            {
                compraDetalle.Estado = compraDetalle.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("comprasDetalles.json", _comprasDetalles);
            }
        }


        // Pagos
        public List<Pago> GetPagos() => _pagos;
        public Pago GetPagoById(int id) => _pagos.FirstOrDefault(p => p.Id == id);
        public void AddPago(Pago pago)
        {
            pago.Id = _pagos.Max(p => p.Id) + 1;
            _pagos.Add(pago);
            SaveToFile("payments.json", _pagos);
        }
        public void UpdatePago(Pago pago)
        {
            var index = _pagos.FindIndex(p => p.Id == pago.Id);
            if (index != -1)
            {
                _pagos[index] = pago;
                SaveToFile("payments.json", _pagos);
            }
        }
        public void DeletePago(int id)
        {
            _pagos.RemoveAll(p => p.Id == id);
            SaveToFile("payments.json", _pagos);
        }

        // Pago Proveedor
        public List<PagoProveedor> GetPagosProveedores() => _pagosProveedores;
        public PagoProveedor GetPagoProveedorById(int id) => _pagosProveedores.FirstOrDefault(p => p.Id == id);
        public void AddPagoProveedor(PagoProveedor pagoProveedor)
        {
            pagoProveedor.Id = _pagosProveedores.Any() ? _pagosProveedores.Max(p => p.Id) + 1 : 1;
            pagoProveedor.Estado = "Activo";
            pagoProveedor.FechaPago = pagoProveedor.FechaPago == default ? DateTime.Now : pagoProveedor.FechaPago;

            // Calculate amount in soles if currency is not PEN
            if (pagoProveedor.Moneda != "PEN" && pagoProveedor.TipoCambio.HasValue && pagoProveedor.TipoCambio > 0)
            {
                pagoProveedor.MontoSoles = pagoProveedor.Monto * pagoProveedor.TipoCambio.Value;
            }
            else
            {
                pagoProveedor.MontoSoles = pagoProveedor.Monto;
            }

            _pagosProveedores.Add(pagoProveedor);
            SaveToFile("pagosProveedores.json", _pagosProveedores);
        }
        public void UpdatePagoProveedor(PagoProveedor pagoProveedor)
        {
            var index = _pagosProveedores.FindIndex(p => p.Id == pagoProveedor.Id);
            if (index != -1)
            {
                // Calculate amount in soles if currency is not PEN
                if (pagoProveedor.Moneda != "PEN" && pagoProveedor.TipoCambio.HasValue && pagoProveedor.TipoCambio > 0)
                {
                    pagoProveedor.MontoSoles = pagoProveedor.Monto * pagoProveedor.TipoCambio.Value;
                }
                else
                {
                    pagoProveedor.MontoSoles = pagoProveedor.Monto;
                }

                _pagosProveedores[index] = pagoProveedor;
                SaveToFile("pagosProveedores.json", _pagosProveedores);
            }
        }
        public void DeletePagoProveedor(int id)
        {
            _pagosProveedores.RemoveAll(p => p.Id == id);
            SaveToFile("pagosProveedores.json", _pagosProveedores);
        }

        public void TogglePagoProveedorEstado(int id)
        {
            var pagoProveedor = _pagosProveedores.FirstOrDefault(p => p.Id == id);
            if (pagoProveedor != null)
            {
                pagoProveedor.Estado = pagoProveedor.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("pagosProveedores.json", _pagosProveedores);
            }
        }

        // Perfiles
        public List<Perfil> GetPerfiles() => _perfiles;
        public Perfil GetPerfilById(int id) => _perfiles.FirstOrDefault(p => p.Id == id);
        public void AddPerfil(Perfil perfil)
        {
            perfil.Id = _perfiles.Max(p => p.Id) + 1;
            _perfiles.Add(perfil);
            SaveToFile("profiles.json", _perfiles);
        }
        public void UpdatePerfil(Perfil perfil)
        {
            var index = _perfiles.FindIndex(p => p.Id == perfil.Id);
            if (index != -1)
            {
                _perfiles[index] = perfil;
                SaveToFile("profiles.json", _perfiles);
            }
        }
        public void DeletePerfil(int id)
        {
            _perfiles.RemoveAll(p => p.Id == id);
            SaveToFile("profiles.json", _perfiles);
        }

        // Solicitudes
        public List<Solicitud> GetSolicitudes() => _solicitudes;
        public Solicitud GetSolicitudById(int id) => _solicitudes.FirstOrDefault(s => s.Id == id);
        public void AddSolicitud(Solicitud solicitud)
        {
            solicitud.Id = _solicitudes.Max(s => s.Id) + 1;
            solicitud.Usuario = _usuarios.FirstOrDefault(u => u.Id == solicitud.IdUsuario);

            // Set default values for purchase request fields if not provided
            solicitud.IdSolicitud = solicitud.IdSolicitud ?? $"SOL-{solicitud.Id:D4}";
            solicitud.AreaSolicitante = solicitud.AreaSolicitante ?? "General";
            solicitud.SolicitadoPor = solicitud.SolicitadoPor ?? solicitud.Usuario?.NombreUsuario ?? "Desconocido";
            solicitud.FechaSolicitud = solicitud.FechaSolicitud == default ? DateTime.Now : solicitud.FechaSolicitud;
            solicitud.Prioridad = solicitud.Prioridad ?? "media";
            solicitud.EstadoSolicitud = solicitud.EstadoSolicitud ?? "Pendiente";

            // Set default values for detalle fields
            solicitud.ProductoServicio = solicitud.ProductoServicio ?? "No especificado";
            solicitud.CantidadSolicitada = solicitud.CantidadSolicitada == 0 ? 1 : solicitud.CantidadSolicitada;
            solicitud.Justificacion = solicitud.Justificacion ?? "Sin justificación";
            solicitud.FechaRequerida = solicitud.FechaRequerida == default ? DateTime.Now.AddDays(7) : solicitud.FechaRequerida;

            // Set default for Descripcion if not provided
            solicitud.Descripcion = solicitud.Descripcion ?? $"Solicitud de {solicitud.ProductoServicio} - {solicitud.Justificacion}";

            _solicitudes.Add(solicitud);
            SaveToFile("requests.json", _solicitudes);
        }
        public void UpdateSolicitud(Solicitud solicitud)
        {
            var index = _solicitudes.FindIndex(s => s.Id == solicitud.Id);
            if (index != -1)
            {
                // Preserve existing values if new values are not provided
                var existingSolicitud = _solicitudes[index];

                // Update only non-null/non-default values
                solicitud.Usuario = _usuarios.FirstOrDefault(u => u.Id == solicitud.IdUsuario);
                solicitud.IdSolicitud = solicitud.IdSolicitud ?? existingSolicitud.IdSolicitud;
                solicitud.AreaSolicitante = solicitud.AreaSolicitante ?? existingSolicitud.AreaSolicitante;
                solicitud.SolicitadoPor = solicitud.SolicitadoPor ?? existingSolicitud.SolicitadoPor;
                solicitud.FechaSolicitud = solicitud.FechaSolicitud == default ? existingSolicitud.FechaSolicitud : solicitud.FechaSolicitud;
                solicitud.Prioridad = solicitud.Prioridad ?? existingSolicitud.Prioridad;
                solicitud.EstadoSolicitud = solicitud.EstadoSolicitud ?? existingSolicitud.EstadoSolicitud;

                // Update detalle fields
                solicitud.ProductoServicio = solicitud.ProductoServicio ?? existingSolicitud.ProductoServicio;
                solicitud.CantidadSolicitada = solicitud.CantidadSolicitada == 0 ? existingSolicitud.CantidadSolicitada : solicitud.CantidadSolicitada;
                solicitud.Justificacion = solicitud.Justificacion ?? existingSolicitud.Justificacion;
                solicitud.FechaRequerida = solicitud.FechaRequerida == default ? existingSolicitud.FechaRequerida : solicitud.FechaRequerida;

                // Update Descripcion field
                solicitud.Descripcion = solicitud.Descripcion ?? existingSolicitud.Descripcion;

                _solicitudes[index] = solicitud;
                SaveToFile("requests.json", _solicitudes);
            }
        }
        public void DeleteSolicitud(int id)
        {
            _solicitudes.RemoveAll(s => s.Id == id);
            SaveToFile("requests.json", _solicitudes);
        }

        // Roles
        public List<Rol> GetRoles() => _roles;
        public Rol GetRolById(int id) => _roles.FirstOrDefault(r => r.Id == id);
        public void AddRol(Rol rol)
        {
            rol.Id = _roles.Max(r => r.Id) + 1;
            _roles.Add(rol);
            SaveToFile("roles.json", _roles);
        }
        public void UpdateRol(Rol rol)
        {
            var index = _roles.FindIndex(r => r.Id == rol.Id);
            if (index != -1)
            {
                _roles[index] = rol;
                SaveToFile("roles.json", _roles);
            }
        }
        public void DeleteRol(int id)
        {
            _roles.RemoveAll(r => r.Id == id);
            SaveToFile("roles.json", _roles);
        }

        // Ventas
        public List<Venta> GetVentas() => _ventas;
        public Venta GetVentaById(int id) => _ventas.FirstOrDefault(v => v.Id == id);
        public void AddVenta(Venta venta)
        {
            venta.Id = _ventas.Max(v => v.Id) + 1;
            venta.Estado = "Activo";
            venta.FechaVenta = DateTime.Now;

            // Calculate total based on product or service
            if (venta.Tipo == "Producto" && venta.IdProducto.HasValue)
            {
                var producto = _productos.FirstOrDefault(p => p.Id == venta.IdProducto.Value);
                if (producto != null)
                {
                    venta.Total = producto.Precio * venta.Cantidad;
                }
            }
            else if (venta.Tipo == "Servicio" && venta.IdServicio.HasValue)
            {
                var servicio = _servicios.FirstOrDefault(s => s.Id == venta.IdServicio.Value);
                if (servicio != null)
                {
                    venta.Total = servicio.Precio * venta.Cantidad;
                }
            }

            _ventas.Add(venta);
            SaveToFile("sales.json", _ventas);
        }
        public void UpdateVenta(Venta venta)
        {
            var index = _ventas.FindIndex(v => v.Id == venta.Id);
            if (index != -1)
            {
                _ventas[index] = venta;
                SaveToFile("sales.json", _ventas);
            }
        }
        public void DeleteVenta(int id)
        {
            _ventas.RemoveAll(v => v.Id == id);
            SaveToFile("sales.json", _ventas);
        }

        // Colegios
        public List<Colegio> GetColegios() => _colegios;
        public Colegio GetColegioById(int id) => _colegios.FirstOrDefault(e => e.Id == id);
        public void AddColegio(Colegio colegio)
        {
            colegio.Id = _colegios.Max(e => e.Id) + 1;
            _colegios.Add(colegio);
            SaveToFile("schools.json", _colegios);
        }
        public void UpdateColegio(Colegio colegio)
        {
            var index = _colegios.FindIndex(e => e.Id == colegio.Id);
            if (index != -1)
            {
                _colegios[index] = colegio;
                SaveToFile("schools.json", _colegios);
            }
        }
        public void DeleteColegio(int id)
        {
            _colegios.RemoveAll(e => e.Id == id);
            SaveToFile("schools.json", _colegios);
        }

        // Servicios
        public List<Servicio> GetServicios() => _servicios;
        public Servicio GetServicioById(int id) => _servicios.FirstOrDefault(s => s.Id == id);
        public void AddServicio(Servicio servicio)
        {
            servicio.Id = _servicios.Max(s => s.Id) + 1;
            _servicios.Add(servicio);
            SaveToFile("services.json", _servicios);
        }
        public void UpdateServicio(Servicio servicio)
        {
            var index = _servicios.FindIndex(s => s.Id == servicio.Id);
            if (index != -1)
            {
                _servicios[index] = servicio;
                SaveToFile("services.json", _servicios);
            }
        }
        public void DeleteServicio(int id)
        {
            _servicios.RemoveAll(s => s.Id == id);
            SaveToFile("services.json", _servicios);
        }

        // Opcion
        public List<MenuItem> GetOpcionItems() => _menuItems;
        public MenuItem GetOpcionItemById(int id) => _menuItems.FirstOrDefault(m => m.Id == id);
        public void AddOpcionItem(MenuItem opcionItem)
        {
            // Initialize collections if null
            opcionItem.Profiles = opcionItem.Profiles ?? new List<int>();
            opcionItem.Roles = opcionItem.Roles ?? new List<int>();
            opcionItem.ActionIds = opcionItem.ActionIds ?? new List<int>();

            opcionItem.Id = _menuItems.Max(m => m.Id) + 1;
            _menuItems.Add(opcionItem);
            SaveToFile("opcion.json", _menuItems);
        }
        public void UpdateOpcionItem(MenuItem opcionItem)
        {
            // Initialize collections if null
            opcionItem.Profiles = opcionItem.Profiles ?? new List<int>();
            opcionItem.Roles = opcionItem.Roles ?? new List<int>();
            opcionItem.ActionIds = opcionItem.ActionIds ?? new List<int>();
            opcionItem.ModuloId = opcionItem.ModuloId;
            var index = _menuItems.FindIndex(m => m.Id == opcionItem.Id);
            if (index != -1)
            {
                _menuItems[index] = opcionItem;
                SaveToFile("opcion.json", _menuItems);
            }
        }
        public void DeleteOpcionItem(int id)
        {
            _menuItems.RemoveAll(m => m.Id == id);
            SaveToFile("opcion.json", _menuItems);
        }

        public void ToggleOpcionItemEstado(int id)
        {
            var opcionItem = _menuItems.FirstOrDefault(m => m.Id == id);
            if (opcionItem != null)
            {
                opcionItem.Estado = opcionItem.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("opcion.json", _menuItems);
            }
        }

        public List<MenuItem> GetOpcionItemsForProfile(int profileId) => _menuItems.Where(m => m.Profiles.Contains(profileId)).ToList();

        // Bulk assignment methods for menu items
        public void BulkAssignOpcionActions(int opcionId, int? rolId, int? perfilId, string[] actionTypes)
        {
            var opcionItem = _menuItems.FirstOrDefault(m => m.Id == opcionId);
            if (opcionItem == null) return;

            // Initialize collections if null
            opcionItem.Roles = opcionItem.Roles ?? new List<int>();
            opcionItem.Profiles = opcionItem.Profiles ?? new List<int>();
            opcionItem.ActionIds = opcionItem.ActionIds ?? new List<int>();

            // Add role if specified
            if (rolId.HasValue && !opcionItem.Roles.Contains(rolId.Value))
            {
                opcionItem.Roles.Add(rolId.Value);
            }

            // Add profile if specified
            if (perfilId.HasValue && !opcionItem.Profiles.Contains(perfilId.Value))
            {
                opcionItem.Profiles.Add(perfilId.Value);
            }

            // Add action IDs
            if (actionTypes != null && actionTypes.Length > 0)
            {
                foreach (var actionType in actionTypes)
                {
                    // Find action by type and add its ID
                    var actionsWithType = _acciones.Where(a => a.Tipo == actionType).ToList();
                    foreach (var action in actionsWithType)
                    {
                        if (!opcionItem.ActionIds.Contains(action.Id))
                        {
                            opcionItem.ActionIds.Add(action.Id);
                        }
                    }
                }
            }

            SaveToFile("opcion.json", _menuItems);
        }

        // Get menu items with actions for specific role/profile
        public List<MenuItem> GetMenuItemsWithActions(int? rolId, int? perfilId)
        {
            if (rolId.HasValue)
            {
                return _menuItems.Where(m => m.Roles.Contains(rolId.Value)).ToList();
            }
            else if (perfilId.HasValue)
            {
                return _menuItems.Where(m => m.Profiles.Contains(perfilId.Value)).ToList();
            }
            return new List<MenuItem>();
        }

        // Acciones
        public List<Accion> GetAcciones() => _acciones;
        public Accion GetAccionById(int id) => _acciones.FirstOrDefault(a => a.Id == id);

        // Method to get action names from IDs for display
        public string GetActionNamesFromIds(List<int> actionIds)
        {

            if (actionIds == null || !actionIds.Any())
                return "N/A";

            var actionNames = new List<string>();
            try
            {
                foreach (var actionId in actionIds)
                {
                    var action = _acciones.FirstOrDefault(a => a.Id == actionId);
                    if (action != null)
                    {
                        actionNames.Add($"{action.Tipo} ({action.Nombre})");
                    }
                }
            }
            catch { }
            return string.Join(", ", actionNames);
        }

        // Method to get profile names from IDs for display
        public string GetProfileNamesFromIds(List<int> profileIds)
        {
            if (profileIds == null || !profileIds.Any())
                return "N/A";

            var profileNames = new List<string>();
            try
            {
                foreach (var profileId in profileIds)
                {
                    var profile = _perfiles.FirstOrDefault(p => p.Id == profileId);
                    if (profile != null)
                    {
                        profileNames.Add(profile.Nombre);
                    }
                }
            }
            catch { }
            return string.Join(", ", profileNames);
        }

        // Method to get role names from IDs for display
        public string GetRoleNamesFromIds(List<int> roleIds)
        {
            if (roleIds == null || !roleIds.Any())
                return "N/A";

            var roleNames = new List<string>();
            try
            {
                foreach (var roleId in roleIds)
                {
                    var role = _roles.FirstOrDefault(r => r.Id == roleId);
                    if (role != null)
                    {
                        roleNames.Add(role.Nombre);
                    }
                }
            }
            catch { }
            return string.Join(", ", roleNames);
        }

        // Method to create opcion items for all actions and assign to admin profile/role
        public void CreateAdminOpcionItems()
        {
            const int adminProfileId = 1; // Admin profile ID
            const int adminRoleId = 1;    // Admin role ID

            // Get all existing actions
            var allActions = GetAcciones();
            if (allActions == null || !allActions.Any())
                return;

            // Create menu items for each action
            foreach (var action in allActions)
            {
                // Check if menu item already exists for this action
                var existingMenuItem = _menuItems.FirstOrDefault(m =>
                    m.ActionIds != null &&
                    m.ActionIds.Contains(action.Id));

                if (existingMenuItem == null)
                {
                    // Create new menu item
                    var menuItem = new MenuItem
                    {
                        Id = _menuItems.Max(m => m.Id) + 1,
                        Title = $"{action.Tipo} {action.Controlador}",
                        Url = $"/{action.Controlador}/{action.AccionMetodo}",
                        Icon = "fas fa-cog", // Default icon
                        Profiles = new List<int> { adminProfileId },
                        Roles = new List<int> { adminRoleId },
                        ActionIds = new List<int> { action.Id },
                        Estado = "Activo"
                    };

                    _menuItems.Add(menuItem);
                }
                else
                {
                    // Add admin profile and role to existing menu item if not already present
                    if (!existingMenuItem.Profiles.Contains(adminProfileId))
                    {
                        existingMenuItem.Profiles.Add(adminProfileId);
                    }
                    if (!existingMenuItem.Roles.Contains(adminRoleId))
                    {
                        existingMenuItem.Roles.Add(adminRoleId);
                    }
                    if (!existingMenuItem.ActionIds.Contains(action.Id))
                    {
                        existingMenuItem.ActionIds.Add(action.Id);
                    }
                }
            }

            SaveToFile("opcion.json", _menuItems);
        }
        public void AddAccion(Accion accion)
        {
            accion.Id = _acciones.Max(a => a.Id) + 1;
            _acciones.Add(accion);
            SaveToFile("actions.json", _acciones);
        }
        public void UpdateAccion(Accion accion)
        {
            var index = _acciones.FindIndex(a => a.Id == accion.Id);
            if (index != -1)
            {
                _acciones[index] = accion;
                SaveToFile("actions.json", _acciones);
            }
        }
        public void DeleteAccion(int id)
        {
            _acciones.RemoveAll(a => a.Id == id);
            SaveToFile("actions.json", _acciones);
        }




        // Activate/Deactivate methods for Actions
        public void ToggleAccionEstado(int id)
        {
            var accion = _acciones.FirstOrDefault(a => a.Id == id);
            if (accion != null)
            {
                accion.Estado = accion.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("actions.json", _acciones);
            }
        }

        // Activate/Deactivate methods
        public void ToggleUsuarioEstado(int id)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario != null)
            {
                usuario.Estado = usuario.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("users.json", _usuarios);
            }
        }

        public void ToggleLibroEstado(int id)
        {
            var libro = _libros.FirstOrDefault(l => l.Id == id);
            if (libro != null)
            {
                libro.Estado = libro.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("books.json", _libros);
            }
        }

        public void TogglePerfilEstado(int id)
        {
            var perfil = _perfiles.FirstOrDefault(p => p.Id == id);
            if (perfil != null)
            {
                perfil.Estado = perfil.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("profiles.json", _perfiles);
            }
        }

        public void ToggleRolEstado(int id)
        {
            var rol = _roles.FirstOrDefault(r => r.Id == id);
            if (rol != null)
            {
                rol.Estado = rol.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("roles.json", _roles);
            }
        }

        public void ToggleSolicitudEstado(int id)
        {
            var solicitud = _solicitudes.FirstOrDefault(s => s.Id == id);
            if (solicitud != null)
            {
                solicitud.Estado = solicitud.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("requests.json", _solicitudes);
            }
        }

        public void ToggleCompraEstado(int id)
        {
            var compra = _compras.FirstOrDefault(p => p.Id == id);
            if (compra != null)
            {
                compra.Estado = compra.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("orders.json", _compras);
            }
        }

        public void TogglePagoEstado(int id)
        {
            var pago = _pagos.FirstOrDefault(p => p.Id == id);
            if (pago != null)
            {
                pago.Estado = pago.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("payments.json", _pagos);
            }
        }

        public void ToggleVentaEstado(int id)
        {
            var venta = _ventas.FirstOrDefault(v => v.Id == id);
            if (venta != null)
            {
                venta.Estado = venta.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("sales.json", _ventas);
            }
        }

        public void ToggleColegioEstado(int id)
        {
            var colegio = _colegios.FirstOrDefault(c => c.Id == id);
            if (colegio != null)
            {
                colegio.Estado = colegio.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("schools.json", _colegios);
            }
        }

        public void ToggleServicioEstado(int id)
        {
            var servicio = _servicios.FirstOrDefault(s => s.Id == id);
            if (servicio != null)
            {
                servicio.Estado = servicio.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("services.json", _servicios);
            }
        }

        // Productos
        public List<Producto> GetProductos() => _productos;
        public Producto GetProductoById(int id) => _productos.FirstOrDefault(p => p.Id == id);
        public void AddProducto(Producto producto)
        {
            producto.Id = _productos.Max(p => p.Id) + 1;
            producto.Estado = "Activo";
            producto.FechaCreacion = DateTime.Now;
            _productos.Add(producto);
            SaveToFile("products.json", _productos);
        }
        public void UpdateProducto(Producto producto)
        {
            var index = _productos.FindIndex(p => p.Id == producto.Id);
            if (index != -1)
            {
                _productos[index] = producto;
                SaveToFile("products.json", _productos);
            }
        }
        public void DeleteProducto(int id)
        {
            _productos.RemoveAll(p => p.Id == id);
            SaveToFile("products.json", _productos);
        }

        public void ToggleProductoEstado(int id)
        {
            var producto = _productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                producto.Estado = producto.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("products.json", _productos);
            }
        }

        // Categorias
        public List<Categoria> GetCategorias() => _categorias;
        public Categoria GetCategoriaById(int id) => _categorias.FirstOrDefault(c => c.Id == id);
        public void AddCategoria(Categoria categoria)
        {
            categoria.Id = _categorias.Max(c => c.Id) + 1;
            categoria.Estado = "Activo";
            _categorias.Add(categoria);
            SaveToFile("categories.json", _categorias);
        }
        public void UpdateCategoria(Categoria categoria)
        {
            var index = _categorias.FindIndex(c => c.Id == categoria.Id);
            if (index != -1)
            {
                _categorias[index] = categoria;
                SaveToFile("categories.json", _categorias);
            }
        }
        public void DeleteCategoria(int id)
        {
            _categorias.RemoveAll(c => c.Id == id);
            SaveToFile("categories.json", _categorias);
        }

        public void ToggleCategoriaEstado(int id)
        {
            var categoria = _categorias.FirstOrDefault(c => c.Id == id);
            if (categoria != null)
            {
                categoria.Estado = categoria.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("categories.json", _categorias);
            }
        }

        // Dashboard methods
        public AdminDashboardModel GetAdminDashboardCounters()
        {
            return new AdminDashboardModel
            {
                TotalUsers = _usuarios?.Count ?? 0,
                ActiveUsers = _usuarios?.Count(u => u.Estado == "Activo") ?? 0,
                TotalBooks = _libros?.Count ?? 0,
                ActiveBooks = _libros?.Count(l => l.Estado == "Activo") ?? 0,
                TotalOrders = _compras?.Count ?? 0,
                PendingOrders = _compras?.Count(p => p.Estado == "Pendiente") ?? 0,
                TotalProfiles = _perfiles?.Count ?? 0,
                ActiveProfiles = _perfiles?.Count(p => p.Estado == "Activo") ?? 0,
                TotalRoles = _roles?.Count ?? 0,
                ActiveRoles = _roles?.Count(r => r.Estado == "Activo") ?? 0,
                TotalMenuItems = _menuItems?.Count ?? 0,
                ActiveMenuItems = _menuItems?.Count(m => m.Estado == "Activo") ?? 0,
                TotalActions = _acciones?.Count ?? 0,
                ActiveActions = _acciones?.Count(a => a.Estado == "Activo") ?? 0
            };
        }

        public GeneralDashboardModel GetGeneralDashboardCounters()
        {
            return new GeneralDashboardModel
            {
                TotalBooks = _libros?.Count ?? 0,
                ActiveBooks = _libros?.Count(l => l.Estado == "Activo") ?? 0,
                TotalOrders = _compras?.Count ?? 0,
                PendingOrders = _compras?.Count(p => p.Estado == "Pendiente") ?? 0,
                TotalServices = _servicios?.Count ?? 0,
                ActiveServices = _servicios?.Count(s => s.Estado == "Activo") ?? 0,
                TotalSchools = _colegios?.Count ?? 0,
                ActiveSchools = _colegios?.Count(c => c.Estado == "Activo") ?? 0
            };
        }

        // Clientes
        public List<Cliente> GetClientes() => _clientes;
        public Cliente GetClienteById(int id) => _clientes.FirstOrDefault(c => c.Id == id);
        public void AddCliente(Cliente cliente)
        {
            cliente.Id = _clientes.Any() ? _clientes.Max(c => c.Id) + 1 : 1;
            cliente.Estado = "Activo";
            _clientes.Add(cliente);
            SaveToFile("clients.json", _clientes);
        }
        public void UpdateCliente(Cliente cliente)
        {
            var index = _clientes.FindIndex(c => c.Id == cliente.Id);
            if (index != -1)
            {
                _clientes[index] = cliente;
                SaveToFile("clients.json", _clientes);
            }
        }
        public void DeleteCliente(int id)
        {
            _clientes.RemoveAll(c => c.Id == id);
            SaveToFile("clients.json", _clientes);
        }

        public void ToggleClienteEstado(int id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                cliente.Estado = cliente.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("clients.json", _clientes);
            }
        }

        // Tipos de Clientes
        public List<TipoCliente> GetTiposClientes() => _tiposClientes;
        public TipoCliente GetTipoClienteById(int id) => _tiposClientes.FirstOrDefault(tc => tc.Id == id);
        public void AddTipoCliente(TipoCliente tipoCliente)
        {
            tipoCliente.Id = _tiposClientes.Any() ? _tiposClientes.Max(tc => tc.Id) + 1 : 1;
            tipoCliente.Estado = "Activo";
            _tiposClientes.Add(tipoCliente);
            SaveToFile("clientTypes.json", _tiposClientes);
        }
        public void UpdateTipoCliente(TipoCliente tipoCliente)
        {
            var index = _tiposClientes.FindIndex(tc => tc.Id == tipoCliente.Id);
            if (index != -1)
            {
                _tiposClientes[index] = tipoCliente;
                SaveToFile("clientTypes.json", _tiposClientes);
            }
        }
        public void DeleteTipoCliente(int id)
        {
            _tiposClientes.RemoveAll(tc => tc.Id == id);
            SaveToFile("clientTypes.json", _tiposClientes);
        }

        public void ToggleTipoClienteEstado(int id)
        {
            var tipoCliente = _tiposClientes.FirstOrDefault(tc => tc.Id == id);
            if (tipoCliente != null)
            {
                tipoCliente.Estado = tipoCliente.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("clientTypes.json", _tiposClientes);
            }
        }

        // Niveles
        public List<Nivel> GetNiveles() => _niveles;
        public Nivel GetNivelById(int id) => _niveles.FirstOrDefault(n => n.Id == id);
        public void AddNivel(Nivel nivel)
        {
            nivel.Id = _niveles.Any() ? _niveles.Max(n => n.Id) + 1 : 1;
            nivel.Estado = "Activo";
            _niveles.Add(nivel);
            SaveToFile("levels.json", _niveles);
        }
        public void UpdateNivel(Nivel nivel)
        {
            var index = _niveles.FindIndex(n => n.Id == nivel.Id);
            if (index != -1)
            {
                _niveles[index] = nivel;
                SaveToFile("levels.json", _niveles);
            }
        }
        public void DeleteNivel(int id)
        {
            _niveles.RemoveAll(n => n.Id == id);
            SaveToFile("levels.json", _niveles);
        }

        public void ToggleNivelEstado(int id)
        {
            var nivel = _niveles.FirstOrDefault(n => n.Id == id);
            if (nivel != null)
            {
                nivel.Estado = nivel.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("levels.json", _niveles);
            }
        }

        // Grados
        public List<Grado> GetGrados() => _grados;
        public Grado GetGradoById(int id) => _grados.FirstOrDefault(g => g.Id == id);
        public void AddGrado(Grado grado)
        {
            grado.Id = _grados.Any() ? _grados.Max(g => g.Id) + 1 : 1;
            grado.Estado = "Activo";
            _grados.Add(grado);
            SaveToFile("grades.json", _grados);
        }
        public void UpdateGrado(Grado grado)
        {
            var index = _grados.FindIndex(g => g.Id == grado.Id);
            if (index != -1)
            {
                _grados[index] = grado;
                SaveToFile("grades.json", _grados);
            }
        }
        public void DeleteGrado(int id)
        {
            _grados.RemoveAll(g => g.Id == id);
            SaveToFile("grades.json", _grados);
        }

        public void ToggleGradoEstado(int id)
        {
            var grado = _grados.FirstOrDefault(g => g.Id == id);
            if (grado != null)
            {
                grado.Estado = grado.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("grades.json", _grados);
            }
        }

        // Libros Grados
        public List<LibroGrado> GetLibrosGrados() => _librosGrados;
        public LibroGrado GetLibroGradoById(int id) => _librosGrados.FirstOrDefault(lg => lg.Id == id);
        public void AddLibroGrado(LibroGrado libroGrado)
        {
            libroGrado.Id = _librosGrados.Any() ? _librosGrados.Max(lg => lg.Id) + 1 : 1;
            libroGrado.Estado = "Activo";
            _librosGrados.Add(libroGrado);
            SaveToFile("bookGrades.json", _librosGrados);
        }
        public void UpdateLibroGrado(LibroGrado libroGrado)
        {
            var index = _librosGrados.FindIndex(lg => lg.Id == libroGrado.Id);
            if (index != -1)
            {
                _librosGrados[index] = libroGrado;
                SaveToFile("bookGrades.json", _librosGrados);
            }
        }
        public void DeleteLibroGrado(int id)
        {
            _librosGrados.RemoveAll(lg => lg.Id == id);
            SaveToFile("bookGrades.json", _librosGrados);
        }

        public void ToggleLibroGradoEstado(int id)
        {
            var libroGrado = _librosGrados.FirstOrDefault(lg => lg.Id == id);
            if (libroGrado != null)
            {
                libroGrado.Estado = libroGrado.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("bookGrades.json", _librosGrados);
            }
        }

        // Modulos CRUD methods
        public List<Modulo> GetModulos() => _modulos;
        public Modulo GetModuloById(int id) => _modulos.FirstOrDefault(m => m.Id == id);
        public void AddModulo(Modulo modulo)
        {
            modulo.Id = _modulos.Any() ? _modulos.Max(m => m.Id) + 1 : 1;
            modulo.Estado = "Activo";
            _modulos.Add(modulo);
            SaveToFile("modulos.json", _modulos);
        }
        public void UpdateModulo(Modulo modulo)
        {
            var index = _modulos.FindIndex(m => m.Id == modulo.Id);
            if (index != -1)
            {
                _modulos[index] = modulo;
                SaveToFile("modulos.json", _modulos);
            }
        }
        public void DeleteModulo(int id)
        {
            _modulos.RemoveAll(m => m.Id == id);
            SaveToFile("modulos.json", _modulos);
        }
        public void ToggleModuloEstado(int id)
        {
            var modulo = _modulos.FirstOrDefault(m => m.Id == id);
            if (modulo != null)
            {
                modulo.Estado = modulo.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("modulos.json", _modulos);
            }
        }

        // Module reordering method
        public void ReorderModulos(List<int> moduleIds)
        {
            // Update the order of modules based on the provided list
            for (int i = 0; i < moduleIds.Count; i++)
            {
                var modulo = _modulos.FirstOrDefault(m => m.Id == moduleIds[i]);
                if (modulo != null)
                {
                    modulo.Orden = i + 1; // Orders start from 1
                }
            }

            // Sort modules by the new order
            _modulos = _modulos.OrderBy(m => m.Orden).ToList();
            SaveToFile("modulos.json", _modulos);
        }

        // Opciones CRUD methods
        public List<Opcion> GetOpciones() => _opciones;
        public Opcion GetOpcionById(int id) => _opciones.FirstOrDefault(o => o.Id == id);
        public List<Opcion> GetOpcionesWithModulos()
        {
            foreach (var opcion in _opciones)
            {
                opcion.Modulo = _modulos.FirstOrDefault(m => m.Id == opcion.ModuloId);
            }
            return _opciones;
        }
        public void AddOpcion(Opcion opcion)
        {
            opcion.Id = _opciones.Any() ? _opciones.Max(o => o.Id) + 1 : 1;
            opcion.Estado = "Activo";
            _opciones.Add(opcion);
            SaveToFile("opciones.json", _opciones);
        }
        public void UpdateOpcion(Opcion opcion)
        {
            var index = _opciones.FindIndex(o => o.Id == opcion.Id);
            if (index != -1)
            {
                _opciones[index] = opcion;
                SaveToFile("opciones.json", _opciones);
            }
        }
        public void DeleteOpcion(int id)
        {
            _opciones.RemoveAll(o => o.Id == id);
            SaveToFile("opciones.json", _opciones);
        }
        public void ToggleOpcionEstado(int id)
        {
            var opcion = _opciones.FirstOrDefault(o => o.Id == id);
            if (opcion != null)
            {
                opcion.Estado = opcion.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("opciones.json", _opciones);
            }
        }

        // Helper methods for Opciones
        public List<string> GetAvailableControllers()
        {
            return new List<string> { "Home", "Usuarios", "Libros", "Compras", "Ventas", "Solicitudes", "Roles", "Perfiles", "Modulos", "Opciones" };
        }

        public List<string> GetAvailableActions()
        {
            return new List<string> { "Index", "Create", "Edit", "Delete", "Details" };
        }

        // Importaciones
        public List<Importacion> GetImportaciones() => _importaciones;
        public Importacion GetImportacionById(int id) => _importaciones.FirstOrDefault(i => i.Id == id);
        public void AddImportacion(Importacion importacion)
        {
            importacion.Id = _importaciones.Any() ? _importaciones.Max(i => i.Id) + 1 : 1;
            importacion.Estado = "Activo";
            importacion.FechaImportacion = importacion.FechaImportacion == default ? DateTime.Now : importacion.FechaImportacion;

            // Calculate totals
            if (importacion.Detalles != null && importacion.Detalles.Any())
            {
                importacion.ValorTotal = importacion.Detalles.Sum(d => d.ValorTotalConImpuestos);
            }

            _importaciones.Add(importacion);
            SaveToFile("importaciones.json", _importaciones);
        }
        public void UpdateImportacion(Importacion importacion)
        {
            var index = _importaciones.FindIndex(i => i.Id == importacion.Id);
            if (index != -1)
            {
                // Calculate totals
                if (importacion.Detalles != null && importacion.Detalles.Any())
                {
                    importacion.ValorTotal = importacion.Detalles.Sum(d => d.ValorTotalConImpuestos);
                }

                _importaciones[index] = importacion;
                SaveToFile("importaciones.json", _importaciones);
            }
        }
        public void DeleteImportacion(int id)
        {
            _importaciones.RemoveAll(i => i.Id == id);
            SaveToFile("importaciones.json", _importaciones);
        }

        public void ToggleImportacionEstado(int id)
        {
            var importacion = _importaciones.FirstOrDefault(i => i.Id == id);
            if (importacion != null)
            {
                importacion.Estado = importacion.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("importaciones.json", _importaciones);
            }
        }

        // Importacion Detalles
        public List<ImportacionDetalle> GetImportacionDetalles() => _importacionesDetalles;
        public ImportacionDetalle GetImportacionDetalleById(int id) => _importacionesDetalles.FirstOrDefault(d => d.Id == id);
        public void AddImportacionDetalle(ImportacionDetalle importacionDetalle)
        {
            // Business logic validation
            if (importacionDetalle.Cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor que 0");

            if (importacionDetalle.PrecioUnitario <= 0)
                throw new ArgumentException("El precio unitario debe ser mayor que 0");

            // Calculate totals
            importacionDetalle.ValorTotal = importacionDetalle.Cantidad * importacionDetalle.PrecioUnitario;
            importacionDetalle.ValorArancel = importacionDetalle.ValorTotal * (importacionDetalle.PorcentajeArancel / 100);
            importacionDetalle.ValorIGV = (importacionDetalle.ValorTotal + importacionDetalle.ValorArancel) * (importacionDetalle.PorcentajeIGV / 100);
            importacionDetalle.ValorTotalConImpuestos = importacionDetalle.ValorTotal + importacionDetalle.ValorArancel + importacionDetalle.ValorIGV;

            importacionDetalle.Id = _importacionesDetalles.Any() ? _importacionesDetalles.Max(id => id.Id) + 1 : 1;
            importacionDetalle.Estado = "Activo";
            _importacionesDetalles.Add(importacionDetalle);
            SaveToFile("importacionesDetalles.json", _importacionesDetalles);
        }
        public void UpdateImportacionDetalle(ImportacionDetalle importacionDetalle)
        {
            var index = _importacionesDetalles.FindIndex(id => id.Id == importacionDetalle.Id);
            if (index != -1)
            {
                _importacionesDetalles[index] = importacionDetalle;
                SaveToFile("importacionesDetalles.json", _importacionesDetalles);
            }
        }
        public void DeleteImportacionDetalle(int id)
        {
            _importacionesDetalles.RemoveAll(d => d.Id == id);
            SaveToFile("importacionesDetalles.json", _importacionesDetalles);
        }

        public void ToggleImportacionDetalleEstado(int id)
        {
            var importacionDetalle = _importacionesDetalles.FirstOrDefault(d => d.Id == id);
            if (importacionDetalle != null)
            {
                importacionDetalle.Estado = importacionDetalle.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("importacionesDetalles.json", _importacionesDetalles);
            }
        }

        // Facturaciones Proveedor
        public List<FacturacionProveedor> GetFacturacionesProveedores() => _facturacionesProveedores;
        public FacturacionProveedor GetFacturacionProveedorById(int id) => _facturacionesProveedores.FirstOrDefault(f => f.Id == id);
        public void AddFacturacionProveedor(FacturacionProveedor facturacion)
        {
            facturacion.Id = _facturacionesProveedores.Any() ? _facturacionesProveedores.Max(f => f.Id) + 1 : 1;
            facturacion.Estado = "Activo";
            facturacion.FechaFactura = facturacion.FechaFactura == default ? DateTime.Now : facturacion.FechaFactura;
            facturacion.FechaVencimiento = facturacion.FechaVencimiento == default ? DateTime.Now.AddDays(30) : facturacion.FechaVencimiento;

            // Calculate totals
            if (facturacion.Detalles != null && facturacion.Detalles.Any())
            {
                facturacion.Subtotal = facturacion.Detalles.Sum(d => d.Subtotal);
                facturacion.ValorIGV = facturacion.Subtotal * (facturacion.PorcentajeIGV / 100);
                facturacion.Total = facturacion.Subtotal + facturacion.ValorIGV;
            }

            _facturacionesProveedores.Add(facturacion);
            SaveToFile("facturacionesProveedores.json", _facturacionesProveedores);
        }
        public void UpdateFacturacionProveedor(FacturacionProveedor facturacion)
        {
            var index = _facturacionesProveedores.FindIndex(f => f.Id == facturacion.Id);
            if (index != -1)
            {
                // Calculate totals
                if (facturacion.Detalles != null && facturacion.Detalles.Any())
                {
                    facturacion.Subtotal = facturacion.Detalles.Sum(d => d.Subtotal);
                    facturacion.ValorIGV = facturacion.Subtotal * (facturacion.PorcentajeIGV / 100);
                    facturacion.Total = facturacion.Subtotal + facturacion.ValorIGV;
                }

                _facturacionesProveedores[index] = facturacion;
                SaveToFile("facturacionesProveedores.json", _facturacionesProveedores);
            }
        }
        public void DeleteFacturacionProveedor(int id)
        {
            _facturacionesProveedores.RemoveAll(f => f.Id == id);
            SaveToFile("facturacionesProveedores.json", _facturacionesProveedores);
        }

        public void ToggleFacturacionProveedorEstado(int id)
        {
            var facturacion = _facturacionesProveedores.FirstOrDefault(f => f.Id == id);
            if (facturacion != null)
            {
                facturacion.Estado = facturacion.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("facturacionesProveedores.json", _facturacionesProveedores);
            }
        }

        // Facturacion Detalles
        public List<FacturacionDetalle> GetFacturacionDetalles() => _facturacionesDetalles;
        public FacturacionDetalle GetFacturacionDetalleById(int id) => _facturacionesDetalles.FirstOrDefault(fd => fd.Id == id);
        public void AddFacturacionDetalle(FacturacionDetalle facturacionDetalle)
        {
            // Business logic validation
            if (facturacionDetalle.Cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor que 0");

            if (facturacionDetalle.PrecioUnitario <= 0)
                throw new ArgumentException("El precio unitario debe ser mayor que 0");

            // Calculate totals
            facturacionDetalle.Subtotal = facturacionDetalle.Cantidad * facturacionDetalle.PrecioUnitario;
            facturacionDetalle.ValorIGV = facturacionDetalle.Subtotal * (facturacionDetalle.PorcentajeIGV / 100);
            facturacionDetalle.Total = facturacionDetalle.Subtotal + facturacionDetalle.ValorIGV;

            facturacionDetalle.Id = _facturacionesDetalles.Any() ? _facturacionesDetalles.Max(fd => fd.Id) + 1 : 1;
            facturacionDetalle.Estado = "Activo";
            _facturacionesDetalles.Add(facturacionDetalle);
            SaveToFile("facturacionesDetalles.json", _facturacionesDetalles);
        }
        public void UpdateFacturacionDetalle(FacturacionDetalle facturacionDetalle)
        {
            var index = _facturacionesDetalles.FindIndex(fd => fd.Id == facturacionDetalle.Id);
            if (index != -1)
            {
                _facturacionesDetalles[index] = facturacionDetalle;
                SaveToFile("facturacionesDetalles.json", _facturacionesDetalles);
            }
        }
        public void DeleteFacturacionDetalle(int id)
        {
            _facturacionesDetalles.RemoveAll(fd => fd.Id == id);
            SaveToFile("facturacionesDetalles.json", _facturacionesDetalles);
        }

        public void ToggleFacturacionDetalleEstado(int id)
        {
            var facturacionDetalle = _facturacionesDetalles.FirstOrDefault(fd => fd.Id == id);
            if (facturacionDetalle != null)
            {
                facturacionDetalle.Estado = facturacionDetalle.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("facturacionesDetalles.json", _facturacionesDetalles);
            }
        }

        // Catalogos
        public List<Catalogo> GetCatalogos() => _catalogos;
        public Catalogo GetCatalogoById(int id) => _catalogos.FirstOrDefault(c => c.Id == id);
        public void AddCatalogo(Catalogo catalogo)
        {
            catalogo.Id = _catalogos.Any() ? _catalogos.Max(c => c.Id) + 1 : 1;
            catalogo.Estado = string.IsNullOrEmpty(catalogo.Estado) ? "Activo" : catalogo.Estado;
            catalogo.FechaRegistro = catalogo.FechaRegistro == default ? DateTime.Now : catalogo.FechaRegistro;
            _catalogos.Add(catalogo);
            SaveToFile("catalogos.json", _catalogos);
        }
        public void UpdateCatalogo(Catalogo catalogo)
        {
            var index = _catalogos.FindIndex(c => c.Id == catalogo.Id);
            if (index != -1)
            {
                _catalogos[index] = catalogo;
                SaveToFile("catalogos.json", _catalogos);
            }
        }
        public void DeleteCatalogo(int id)
        {
            _catalogos.RemoveAll(c => c.Id == id);
            SaveToFile("catalogos.json", _catalogos);
        }

        public void ToggleCatalogoEstado(int id)
        {
            var catalogo = _catalogos.FirstOrDefault(c => c.Id == id);
            if (catalogo != null)
            {
                catalogo.Estado = catalogo.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("catalogos.json", _catalogos);
            }
        }

        // Recetas
        private List<Receta> _recetas;

        public List<Receta> GetRecetas() => _recetas;
        public Receta GetRecetaById(int id) => _recetas.FirstOrDefault(r => r.Id == id);
        public void AddReceta(Receta receta)
        {
            receta.Id = _recetas.Any() ? _recetas.Max(r => r.Id) + 1 : 1;
            receta.Estado = string.IsNullOrEmpty(receta.Estado) ? "Activo" : receta.Estado;
            receta.FechaCreacion = receta.FechaCreacion == default ? DateTime.Now : receta.FechaCreacion;
            _recetas.Add(receta);
            SaveToFile("recetas.json", _recetas);
        }
        public void UpdateReceta(Receta receta)
        {
            var index = _recetas.FindIndex(r => r.Id == receta.Id);
            if (index != -1)
            {
                _recetas[index] = receta;
                SaveToFile("recetas.json", _recetas);
            }
        }
        public void DeleteReceta(int id)
        {
            _recetas.RemoveAll(r => r.Id == id);
            SaveToFile("recetas.json", _recetas);
        }

        public void ToggleRecetaEstado(int id)
        {
            var receta = _recetas.FirstOrDefault(r => r.Id == id);
            if (receta != null)
            {
                receta.Estado = receta.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("recetas.json", _recetas);
            }
        }

        // ProgramacionesOrdenesProduccion
        public List<ProgramacionOrdenProduccion> GetProgramacionesOrdenesProduccion() => _programacionesOrdenesProduccion;
        public ProgramacionOrdenProduccion GetProgramacionOrdenProduccionById(int id) => _programacionesOrdenesProduccion.FirstOrDefault(p => p.Id == id);
        public void AddProgramacionOrdenProduccion(ProgramacionOrdenProduccion programacion)
        {
            programacion.Id = _programacionesOrdenesProduccion.Any() ? _programacionesOrdenesProduccion.Max(p => p.Id) + 1 : 1;
            programacion.Estado = string.IsNullOrEmpty(programacion.Estado) ? "Activo" : programacion.Estado;
            programacion.FechaCreacion = programacion.FechaCreacion == default ? DateTime.Now : programacion.FechaCreacion;
            _programacionesOrdenesProduccion.Add(programacion);
            SaveToFile("programacionesOrdenesProduccion.json", _programacionesOrdenesProduccion);
        }
        public void UpdateProgramacionOrdenProduccion(ProgramacionOrdenProduccion programacion)
        {
            var index = _programacionesOrdenesProduccion.FindIndex(p => p.Id == programacion.Id);
            if (index != -1)
            {
                _programacionesOrdenesProduccion[index] = programacion;
                SaveToFile("programacionesOrdenesProduccion.json", _programacionesOrdenesProduccion);
            }
        }
        public void DeleteProgramacionOrdenProduccion(int id)
        {
            _programacionesOrdenesProduccion.RemoveAll(p => p.Id == id);
            SaveToFile("programacionesOrdenesProduccion.json", _programacionesOrdenesProduccion);
        }

        public void ToggleProgramacionOrdenProduccionEstado(int id)
        {
            var programacion = _programacionesOrdenesProduccion.FirstOrDefault(p => p.Id == id);
            if (programacion != null)
            {
                programacion.Estado = programacion.Estado == "Activo" ? "Inactivo" : "Activo";
                SaveToFile("programacionesOrdenesProduccion.json", _programacionesOrdenesProduccion);
            }
        }
    }
}