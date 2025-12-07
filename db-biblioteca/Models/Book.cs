using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public int Cantidad { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}