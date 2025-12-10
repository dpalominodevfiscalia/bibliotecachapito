using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Estado { get; set; } = "Activo";

        // Computed property for full name
        public string NombreCompleto => $"{Nombre} {Apellido}".Trim();

        // Navigation properties for relationships
        public int? TipoClienteId { get; set; }
        public virtual TipoCliente TipoCliente { get; set; }

        public virtual ICollection<Nivel> Niveles { get; set; }
    }
}