using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class ImportacionDetalle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID de importación es obligatorio")]
        public int IdImportacion { get; set; }
        public virtual Importacion Importacion { get; set; }

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

        [Required(ErrorMessage = "El valor total es obligatorio")]
        [Range(0.01, 1000000, ErrorMessage = "El valor total debe estar entre 0.01 y 1000000")]
        public decimal ValorTotal { get; set; }

        [StringLength(50, ErrorMessage = "La partida arancelaria no puede exceder 50 caracteres")]
        public string PartidaArancelaria { get; set; }

        [Range(0, 100, ErrorMessage = "El porcentaje de arancel debe estar entre 0 y 100")]
        public decimal PorcentajeArancel { get; set; }

        [Range(0, 1000000, ErrorMessage = "El valor de arancel debe estar entre 0 y 1000000")]
        public decimal ValorArancel { get; set; }

        [Range(0, 100, ErrorMessage = "El porcentaje de IGV debe estar entre 0 y 100")]
        public decimal PorcentajeIGV { get; set; }

        [Range(0, 1000000, ErrorMessage = "El valor de IGV debe estar entre 0 y 1000000")]
        public decimal ValorIGV { get; set; }

        [Range(0, 1000000, ErrorMessage = "El valor total con impuestos debe estar entre 0 y 1000000")]
        public decimal ValorTotalConImpuestos { get; set; }

        public string Estado { get; set; } = "Activo";
    }
}