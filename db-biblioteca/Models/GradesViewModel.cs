using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class GradesViewModel
    {
        // Main data - list of grades
        public IEnumerable<Grado> Grados { get; set; }

        // Related data for dropdowns and lookups
        public IEnumerable<Nivel> Niveles { get; set; }

        // Current filter information
        public int? CurrentNivelId { get; set; }
        public string CurrentNivelNombre { get; set; }

        // Constructor
        public GradesViewModel()
        {
            Grados = new List<Grado>();
            Niveles = new List<Nivel>();
        }
    }
}