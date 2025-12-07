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
        _categorias = LoadFromFile<Categoria>("categories.json");
        PopulateUsuarioDetails();
        PopulateSolicitudUsuario();
        PopulateAccionRelations();
        PopulateVentaRelations();
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
        compra.Id = _compras.Max(o => o.Id) + 1;
        _compras.Add(compra);
        SaveToFile("orders.json", _compras);
    }
    public void UpdateCompra(Compra compra)
    {
        var index = _compras.FindIndex(o => o.Id == compra.Id);
        if (index != -1)
        {
            _compras[index] = compra;
            SaveToFile("orders.json", _compras);
        }
    }
    public void DeleteCompra(int id)
    {
        _compras.RemoveAll(o => o.Id == id);
        SaveToFile("orders.json", _compras);
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
        foreach (var actionId in actionIds)
        {
            var action = _acciones.FirstOrDefault(a => a.Id == actionId);
            if (action != null)
            {
                actionNames.Add($"{action.Tipo} ({action.Nombre})");
            }
        }
        return string.Join(", ", actionNames);
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
}
}