using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Nivel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; } = "Activo";

        // Foreign key and navigation property for Client Type
        public int TipoClienteId { get; set; }
        public virtual TipoCliente TipoCliente { get; set; }

        // Navigation property for Grades
        public virtual ICollection<Grado> Grados { get; set; }
    }
}