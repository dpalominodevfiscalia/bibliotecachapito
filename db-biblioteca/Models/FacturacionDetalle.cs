using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class FacturacionDetalle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID de facturación es obligatorio")]
        public int IdFacturacion { get; set; }
        public virtual FacturacionProveedor Facturacion { get; set; }

        [Required(ErrorMessage = "El ID de producto es obligatorio")]
        public int IdProducto { get; set; }
        public virtual Producto Producto { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, 10000, ErrorMessage = "La cantidad debe estar entre 1 y 10000")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Range(0.01, 100000, ErrorMessage = "El precio debe estar entre 0.01 y 100000")]
        public decimal PrecioUnitario { get; set; }

        [Required(ErrorMessage = "El subtotal es obligatorio")]
        [Range(0.01, 1000000, ErrorMessage = "El subtotal debe estar entre 0.01 y 1000000")]
        public decimal Subtotal { get; set; }

        [Range(0, 100, ErrorMessage = "El porcentaje de IGV debe estar entre 0 y 100")]
        public decimal PorcentajeIGV { get; set; } = 18;

        [Range(0, 1000000, ErrorMessage = "El valor de IGV debe estar entre 0 y 1000000")]
        public decimal ValorIGV { get; set; }

        [Range(0, 1000000, ErrorMessage = "El total debe estar entre 0 y 1000000")]
        public decimal Total { get; set; }

        [StringLength(50, ErrorMessage = "El código de producto no puede exceder 50 caracteres")]
        public string CodigoProducto { get; set; }

        public string Estado { get; set; } = "Activo";
    }
}