using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaDB.Models
{
    public class MovimientoAlmacen
    {
        public int Id { get; set; }

        [Required]
        public int IdTipoMovimiento { get; set; }

        [Required]
        public int IdAlmacenOrigen { get; set; }

        public int? IdAlmacenDestino { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        public decimal Cantidad { get; set; }

        [StringLength(200)]
        public string Observaciones { get; set; }

        [Required]
        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Usuario { get; set; }

        [StringLength(50)]
        public string DocumentoReferencia { get; set; }

        public bool Estado { get; set; } = true;

        // Navigation properties
        [ForeignKey("IdTipoMovimiento")]
        public virtual TipoMovimiento TipoMovimiento { get; set; }

        [ForeignKey("IdAlmacenOrigen")]
        public virtual Almacen AlmacenOrigen { get; set; }

        [ForeignKey("IdAlmacenDestino")]
        public virtual Almacen AlmacenDestino { get; set; }

        [ForeignKey("IdProducto")]
        public virtual Producto Producto { get; set; }
    }
}