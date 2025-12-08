using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class LevelsViewModel
    {
        // Main data - list of levels
        public IEnumerable<Nivel> Niveles { get; set; }

        // Filter data
        public IEnumerable<TipoCliente> TiposClientes { get; set; }

        // Current filter selection
        public int? TipoClienteId { get; set; }
        public string TipoClienteNombre { get; set; }

        // Constructor
        public LevelsViewModel()
        {
            TiposClientes = new List<TipoCliente>();
            Niveles = new List<Nivel>();
        }
    }
}