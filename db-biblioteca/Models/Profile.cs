using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Perfil
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El número de documento es obligatorio")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El número de documento debe ser solo numérico")]
        public string NumeroDocumento { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El teléfono debe ser solo numérico")]
        public string Telefono { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<Accion> Acciones { get; set; }
        public int? StartMenuId { get; set; } // ID del menú de inicio para este perfil
        public string Estado { get; set; } = "Activo";
    }
}