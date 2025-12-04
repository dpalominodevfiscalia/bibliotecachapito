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
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Perfil> Perfiles { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
    }
}