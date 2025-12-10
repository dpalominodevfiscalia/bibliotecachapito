using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Importacion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de importación es obligatorio")]
        [StringLength(50, ErrorMessage = "El número de importación no puede exceder 50 caracteres")]
        public string NumeroImportacion { get; set; }

        [Required(ErrorMessage = "El ID de proveedor es obligatorio")]
        public int IdProveedor { get; set; }
        public virtual Proveedor Proveedor { get; set; }

        [Required(ErrorMessage = "La fecha de importación es obligatoria")]
        public DateTime FechaImportacion { get; set; }

        [Required(ErrorMessage = "El país de origen es obligatorio")]
        [StringLength(100, ErrorMessage = "El país de origen no puede exceder 100 caracteres")]
        public string PaisOrigen { get; set; }

        [Required(ErrorMessage = "El puerto de entrada es obligatorio")]
        [StringLength(100, ErrorMessage = "El puerto de entrada no puede exceder 100 caracteres")]
        public string PuertoEntrada { get; set; }

        [Required(ErrorMessage = "El incoterm es obligatorio")]
        [StringLength(50, ErrorMessage = "El incoterm no puede exceder 50 caracteres")]
        public string Incoterm { get; set; }

        [StringLength(100, ErrorMessage = "El medio de transporte no puede exceder 100 caracteres")]
        public string MedioTransporte { get; set; }

        [StringLength(100, ErrorMessage = "El número de guía no puede exceder 100 caracteres")]
        public string NumeroGuia { get; set; }

        [StringLength(100, ErrorMessage = "El número de factura no puede exceder 100 caracteres")]
        public string NumeroFactura { get; set; }

        [Range(0, 1000000, ErrorMessage = "El valor FOB debe estar entre 0 y 1000000")]
        public decimal ValorFOB { get; set; }

        [Range(0, 1000000, ErrorMessage = "El valor CIF debe estar entre 0 y 1000000")]
        public decimal ValorCIF { get; set; }

        [Range(0, 100, ErrorMessage = "El porcentaje de arancel debe estar entre 0 y 100")]
        public decimal PorcentajeArancel { get; set; }

        [Range(0, 1000000, ErrorMessage = "El valor de arancel debe estar entre 0 y 1000000")]
        public decimal ValorArancel { get; set; }

        [Range(0, 100, ErrorMessage = "El porcentaje de IGV debe estar entre 0 y 100")]
        public decimal PorcentajeIGV { get; set; }

        [Range(0, 1000000, ErrorMessage = "El valor de IGV debe estar entre 0 y 1000000")]
        public decimal ValorIGV { get; set; }

        [Range(0, 1000000, ErrorMessage = "El valor total debe estar entre 0 y 1000000")]
        public decimal ValorTotal { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string Observaciones { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; } = "Activo";

        public string EstadoAduana { get; set; } = "En trámite";

        public DateTime? FechaLlegada { get; set; }
        public DateTime? FechaDespacho { get; set; }

        public virtual List<ImportacionDetalle> Detalles { get; set; } = new List<ImportacionDetalle>();
    }
}