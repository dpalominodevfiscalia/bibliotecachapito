using BibliotecaDB.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BibliotecaDB.Services;

public class DataService
{
    private readonly string _dataPath;
    private List<Usuario> _usuarios;
    private List<Libro> _libros;
    private List<Pedido> _pedidos;
    private List<Pago> _pagos;
    private List<Perfil> _perfiles;
    private List<Solicitud> _solicitudes;
    private List<Rol> _roles;
    private List<Venta> _ventas;
    private List<Colegio> _colegios;
    private List<Servicio> _servicios;
    private List<MenuItem> _menuItems;

    public DataService(IWebHostEnvironment env)
    {
        _dataPath = Path.Combine(env.WebRootPath, "Data");
        LoadData();
    }

    private void LoadData()
    {
        _usuarios = LoadFromFile<Usuario>("users.json");
        _libros = LoadFromFile<Libro>("books.json");
        _pedidos = LoadFromFile<Pedido>("orders.json");
        _pagos = LoadFromFile<Pago>("payments.json");
        _perfiles = LoadFromFile<Perfil>("profiles.json");
        _solicitudes = LoadFromFile<Solicitud>("requests.json");
        _roles = LoadFromFile<Rol>("roles.json");
        _ventas = LoadFromFile<Venta>("sales.json");
        _colegios = LoadFromFile<Colegio>("schools.json");
        _servicios = LoadFromFile<Servicio>("services.json");
        _menuItems = LoadFromFile<MenuItem>("menu.json");
        PopulateUsuarioDetails();
    }

    private void PopulateUsuarioDetails()
    {
        foreach (var usuario in _usuarios)
        {
            usuario.Rol = _roles.FirstOrDefault(r => r.Id == usuario.IdRol);
            usuario.Perfil = _perfiles.FirstOrDefault(p => p.Id == usuario.IdPerfil);
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

    // Pedidos
    public List<Pedido> GetPedidos() => _pedidos;
    public Pedido GetPedidoById(int id) => _pedidos.FirstOrDefault(o => o.Id == id);
    public void AddPedido(Pedido pedido)
    {
        pedido.Id = _pedidos.Max(o => o.Id) + 1;
        _pedidos.Add(pedido);
        SaveToFile("orders.json", _pedidos);
    }
    public void UpdatePedido(Pedido pedido)
    {
        var index = _pedidos.FindIndex(o => o.Id == pedido.Id);
        if (index != -1)
        {
            _pedidos[index] = pedido;
            SaveToFile("orders.json", _pedidos);
        }
    }
    public void DeletePedido(int id)
    {
        _pedidos.RemoveAll(o => o.Id == id);
        SaveToFile("orders.json", _pedidos);
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
        _solicitudes.Add(solicitud);
        SaveToFile("requests.json", _solicitudes);
    }
    public void UpdateSolicitud(Solicitud solicitud)
    {
        var index = _solicitudes.FindIndex(s => s.Id == solicitud.Id);
        if (index != -1)
        {
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

    // Menu
    public List<MenuItem> GetMenuItemsForProfile(int profileId) => _menuItems.Where(m => m.Profiles.Contains(profileId)).ToList();
}