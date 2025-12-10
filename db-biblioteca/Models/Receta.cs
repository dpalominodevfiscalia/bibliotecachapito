using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaDB.Models
{
    public class Receta
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CodigoReceta { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        [StringLength(50)]
        public string Categoria { get; set; }

        [StringLength(50)]
        public string Tipo { get; set; }

        public int? TiempoPreparacion { get; set; } // in minutes

        public int? Porciones { get; set; }

        [StringLength(50)]
        public string Dificultad { get; set; }

        [StringLength(50)]
        public string Estado { get; set; } = "Activo";

        [StringLength(100)]
        public string CreadoPor { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string ModificadoPor { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}