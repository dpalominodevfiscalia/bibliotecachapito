using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioCompra { get; set; }
        public int Stock { get; set; }
        public int IdCategoria { get; set; }
        public virtual Categoria Categoria { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Activo";
        public string ImagenUrl { get; set; }
    }
}