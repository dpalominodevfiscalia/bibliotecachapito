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

        // Module and Option management
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<Opcion> Opciones { get; set; }
        public DbSet<OpcionAccion> OpcionAcciones { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        // Warehouse management
        public DbSet<Almacen> Almacenes { get; set; }
        public DbSet<TipoMovimiento> TiposMovimientos { get; set; }
        public DbSet<MovimientoAlmacen> MovimientosAlmacenes { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        // Catalog management
        public DbSet<Catalogo> Catalogos { get; set; }

        // Recipe management
        public DbSet<Receta> Recetas { get; set; }

        // Production Order Scheduling
        public DbSet<ProgramacionOrdenProduccion> ProgramacionesOrdenesProduccion { get; set; }
    }
}