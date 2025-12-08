using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class LibroGrado
    {
        public int Id { get; set; }

        // Foreign keys
        public int LibroId { get; set; }
        public int GradoId { get; set; }

        // Navigation properties
        public virtual Libro Libro { get; set; }
        public virtual Grado Grado { get; set; }

        // Additional properties for the relationship
        public int Cantidad { get; set; } = 1;
        public string Estado { get; set; } = "Activo";
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;
    }
}