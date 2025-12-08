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
        public int? IdProveedor { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        public int? IdCondicionPago { get; set; }
        public virtual CondicionPago CondicionPago { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Activo";
        public string NumeroOrdenCompra { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string Observaciones { get; set; }
        public virtual List<CompraDetalle> Detalles { get; set; } = new List<CompraDetalle>();
    }
}