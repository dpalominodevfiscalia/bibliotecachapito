using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Venta
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "El usuario es requerido")]
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        [Required(ErrorMessage = "La fecha de venta es requerida")]
        public DateTime FechaVenta { get; set; }

        [Required(ErrorMessage = "El total es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor que 0")]
        public decimal Total { get; set; }

        public string Estado { get; set; } = "Activo";

        //[Required(ErrorMessage = "El producto o servicio es requerido")]
        public int? IdProducto { get; set; }
        public virtual Producto Producto { get; set; }

        //[Required(ErrorMessage = "El producto o servicio es requerido")]
        public int? IdServicio { get; set; }
        public virtual Servicio Servicio { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; } = 1;

        [Required(ErrorMessage = "El tipo de venta es requerido")]
        public string Tipo { get; set; } = "Producto"; // "Producto" o "Servicio"
    }
}