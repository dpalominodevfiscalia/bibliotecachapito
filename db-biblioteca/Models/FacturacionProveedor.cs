using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class FacturacionProveedor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de factura es obligatorio")]
        [StringLength(50, ErrorMessage = "El número de factura no puede exceder 50 caracteres")]
        public string NumeroFactura { get; set; }

        [Required(ErrorMessage = "El ID de proveedor es obligatorio")]
        public int IdProveedor { get; set; }
        public virtual Proveedor Proveedor { get; set; }

        [Required(ErrorMessage = "La fecha de factura es obligatoria")]
        public DateTime FechaFactura { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        public DateTime FechaVencimiento { get; set; }

        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [StringLength(50, ErrorMessage = "El tipo de documento no puede exceder 50 caracteres")]
        public string TipoDocumento { get; set; } = "Factura";

        [Required(ErrorMessage = "El subtotal es obligatorio")]
        [Range(0, 1000000, ErrorMessage = "El subtotal debe estar entre 0 y 1000000")]
        public decimal Subtotal { get; set; }

        [Range(0, 100, ErrorMessage = "El porcentaje de IGV debe estar entre 0 y 100")]
        public decimal PorcentajeIGV { get; set; } = 18;

        [Range(0, 1000000, ErrorMessage = "El valor de IGV debe estar entre 0 y 1000000")]
        public decimal ValorIGV { get; set; }

        [Range(0, 1000000, ErrorMessage = "El total debe estar entre 0 y 1000000")]
        public decimal Total { get; set; }

        [StringLength(100, ErrorMessage = "La moneda no puede exceder 100 caracteres")]
        public string Moneda { get; set; } = "PEN";

        [StringLength(100, ErrorMessage = "El tipo de cambio no puede exceder 100 caracteres")]
        public string TipoCambio { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string Observaciones { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; } = "Pendiente";

        public string EstadoPago { get; set; } = "Pendiente";

        public DateTime? FechaPago { get; set; }

        [StringLength(100, ErrorMessage = "El método de pago no puede exceder 100 caracteres")]
        public string MetodoPago { get; set; }

        [StringLength(100, ErrorMessage = "El número de operación no puede exceder 100 caracteres")]
        public string NumeroOperacion { get; set; }

        public int? IdCompra { get; set; }
        public virtual Compra Compra { get; set; }

        public int? IdImportacion { get; set; }
        public virtual Importacion Importacion { get; set; }

        public virtual List<FacturacionDetalle> Detalles { get; set; } = new List<FacturacionDetalle>();
    }
}