using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaDB.Models
{
    public class Almacen
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        [StringLength(100)]
        public string Ubicacion { get; set; }

        [StringLength(50)]
        public string Responsable { get; set; }

        public bool Estado { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<MovimientoAlmacen> MovimientosEntrada { get; set; }
        public virtual ICollection<MovimientoAlmacen> MovimientosSalida { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}