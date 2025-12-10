using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class PagoProveedor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID de proveedor es obligatorio")]
        [Display(Name = "Proveedor")]
        public int IdProveedor { get; set; }

        public virtual Proveedor Proveedor { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que 0")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La fecha de pago es obligatoria")]
        [Display(Name = "Fecha de Pago")]
        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        [StringLength(50, ErrorMessage = "El método de pago no puede exceder 50 caracteres")]
        [Display(Name = "Método de Pago")]
        public string MetodoPago { get; set; }

        [StringLength(100, ErrorMessage = "La referencia no puede exceder 100 caracteres")]
        [Display(Name = "Referencia")]
        public string Referencia { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [StringLength(100, ErrorMessage = "El número de factura no puede exceder 100 caracteres")]
        [Display(Name = "Número de Factura")]
        public string NumeroFactura { get; set; }

        [Required(ErrorMessage = "La moneda es obligatoria")]
        [StringLength(10, ErrorMessage = "La moneda no puede exceder 10 caracteres")]
        [Display(Name = "Moneda")]
        public string Moneda { get; set; } = "PEN";

        [Display(Name = "Tipo de Cambio")]
        public decimal? TipoCambio { get; set; }

        [Display(Name = "Monto en Soles")]
        public decimal? MontoSoles { get; set; }

        public string Estado { get; set; } = "Activo";
    }
}