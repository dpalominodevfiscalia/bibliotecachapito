using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaDB.Models
{
    public class TipoMovimiento
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string Descripcion { get; set; }

        [Required]
        public bool EsEntrada { get; set; } // true = Entrada, false = Salida

        public bool Estado { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<MovimientoAlmacen> Movimientos { get; set; }
    }
}