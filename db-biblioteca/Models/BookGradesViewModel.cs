using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class BookGradesViewModel
    {
        // Main data - list of book-grade relationships
        public IEnumerable<LibroGrado> LibroGrados { get; set; }

        // Related data for dropdowns
        public IEnumerable<Libro> Libros { get; set; }
        public IEnumerable<Grado> Grados { get; set; }

        // Current filter information
        public int? CurrentGradoId { get; set; }
        public string CurrentGradoNombre { get; set; }

        // Constructor
        public BookGradesViewModel()
        {
            LibroGrados = new List<LibroGrado>();
            Libros = new List<Libro>();
            Grados = new List<Grado>();
        }
    }
}