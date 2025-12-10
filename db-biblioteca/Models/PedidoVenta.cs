using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaDB.Models
{
    public class PedidoVenta
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NumeroPedido { get; set; }

        public int? IdCliente { get; set; }

        public int? IdCotizacion { get; set; }

        [Required]
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        [Required]
        public DateTime FechaEntrega { get; set; }

        [StringLength(100)]
        public string CondicionesPago { get; set; }

        [StringLength(100)]
        public string Moneda { get; set; } = "PEN";

        public decimal Subtotal { get; set; } = 0;

        public decimal Impuestos { get; set; } = 0;

        public decimal Total { get; set; } = 0;

        [StringLength(200)]
        public string Observaciones { get; set; }

        [StringLength(50)]
        public string Estado { get; set; } = "Pendiente";

        [StringLength(50)]
        public string Vendedor { get; set; }

        // Navigation properties
        public virtual Cliente Cliente { get; set; }
        public virtual Cotizacion Cotizacion { get; set; }
    }
}