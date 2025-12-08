using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Grado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; } = "Activo";

        // Foreign key and navigation property for Level
        public int NivelId { get; set; }
        public virtual Nivel Nivel { get; set; }

        // Navigation property for Books
        public virtual ICollection<LibroGrado> LibrosGrados { get; set; }
    }
}