using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class CompraDetalle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID de compra es obligatorio")]
        public int IdCompra { get; set; }
        public virtual Compra Compra { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        public int IdProducto { get; set; }
        public virtual Producto Producto { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La unidad de medida es obligatoria")]
        public string UnidadMedida { get; set; }

        [Required(ErrorMessage = "La cantidad solicitada es obligatoria")]
        [Range(1, 10000, ErrorMessage = "La cantidad debe estar entre 1 y 10000")]
        public int CantidadSolicitada { get; set; }

        [Range(0, 10000, ErrorMessage = "La cantidad recibida debe estar entre 0 y 10000")]
        public int CantidadRecibida { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Range(0.01, 100000, ErrorMessage = "El precio debe estar entre 0.01 y 100000")]
        public decimal PrecioUnitario { get; set; }

        [Required(ErrorMessage = "El total parcial es obligatorio")]
        [Range(0.01, 1000000, ErrorMessage = "El total debe estar entre 0.01 y 1000000")]
        public decimal TotalParcial { get; set; }

        public string Estado { get; set; } = "Activo";
    }
}