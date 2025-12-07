using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Accion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio")]
        public string Tipo { get; set; } // "Crear", "Editar", "Eliminar", etc.

        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; } // "Activo", "Inactivo", "Pendiente", etc.

        [Required(ErrorMessage = "El controlador es obligatorio")]
        public string Controlador { get; set; } // Nombre del controlador al que pertenece

        [Required(ErrorMessage = "La acción es obligatoria")]
        public string AccionMetodo { get; set; } // Nombre del método de acción
    }
}