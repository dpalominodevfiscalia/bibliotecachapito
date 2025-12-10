using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaDB.Models
{
    public class Cobranza
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NumeroRecibo { get; set; }

        public int? IdCliente { get; set; }

        public int? IdPedidoVenta { get; set; }

        [Required]
        public DateTime FechaCobranza { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Moneda { get; set; } = "PEN";

        [Required]
        public decimal Monto { get; set; } = 0;

        [StringLength(100)]
        public string FormaPago { get; set; }

        [StringLength(100)]
        public string ReferenciaPago { get; set; }

        [StringLength(200)]
        public string Observaciones { get; set; }

        [StringLength(50)]
        public string Estado { get; set; } = "Pendiente";

        [StringLength(50)]
        public string Cobrador { get; set; }

        // Navigation properties
        public virtual Cliente Cliente { get; set; }
        public virtual PedidoVenta PedidoVenta { get; set; }
    }
}