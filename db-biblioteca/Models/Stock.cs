using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaDB.Models
{
    public class Stock
    {
        public int Id { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        public int IdAlmacen { get; set; }

        [Required]
        public decimal Cantidad { get; set; }

        public decimal StockMinimo { get; set; } = 0;

        public decimal StockMaximo { get; set; } = 0;

        public DateTime FechaActualizacion { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string UsuarioActualizacion { get; set; }

        // Navigation properties
        [ForeignKey("IdProducto")]
        public virtual Producto Producto { get; set; }

        [ForeignKey("IdAlmacen")]
        public virtual Almacen Almacen { get; set; }
    }
}