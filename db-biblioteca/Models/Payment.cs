using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int IdCompra { get; set; }
        public virtual Compra Compra { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string Metodo { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}