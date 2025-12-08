using Microsoft.EntityFrameworkCore;

namespace BibliotecaDB.Models
{
    public class BibliotecaDBContext : DbContext
    {
        public BibliotecaDBContext(DbContextOptions<BibliotecaDBContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Colegio> Colegios { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Perfil> Perfiles { get; set; }
        public DbSet<Servicio> Servicios { get; set; }

        // New entities for Client management system
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<TipoCliente> TiposClientes { get; set; }
        public DbSet<Nivel> Niveles { get; set; }
        public DbSet<Grado> Grados { get; set; }
        public DbSet<LibroGrado> LibrosGrados { get; set; }
    }
}