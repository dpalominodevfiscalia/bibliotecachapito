using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}