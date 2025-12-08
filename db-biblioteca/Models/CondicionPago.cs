using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class CondicionPago
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Los días de crédito son obligatorios")]
        [Range(0, 365, ErrorMessage = "Los días de crédito deben estar entre 0 y 365")]
        public int DiasCredito { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio")]
        public string Tipo { get; set; }

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100%")]
        public decimal DescuentoPorProntoPago { get; set; }

        [Required(ErrorMessage = "La forma de pago es obligatoria")]
        public string FormaPago { get; set; }

        [Required(ErrorMessage = "La moneda es obligatoria")]
        public string Moneda { get; set; }

        [Required(ErrorMessage = "Debe especificar si aplica impuestos")]
        public string AplicaImpuestos { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string Observaciones { get; set; }

        public string Estado { get; set; } = "Activo";
    }
}