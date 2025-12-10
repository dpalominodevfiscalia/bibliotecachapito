using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaDB.Models
{
    public class Modulo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del módulo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; }

        [StringLength(50, ErrorMessage = "El icono no puede exceder 50 caracteres")]
        public string Icono { get; set; } = "fas fa-cube";

        [StringLength(100, ErrorMessage = "La ruta no puede exceder 100 caracteres")]
        public string Ruta { get; set; }

        [Range(0, 999, ErrorMessage = "El orden debe estar entre 0 y 999")]
        public int Orden { get; set; } = 0;

        [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
        public string Estado { get; set; } = "Activo";

        public List<string> Permisos { get; set; } = new List<string>();

        // Navigation properties
        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}