using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class OpcionAccion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la acción es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(100, ErrorMessage = "La descripción no puede exceder 100 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El código de permiso es obligatorio")]
        [StringLength(50, ErrorMessage = "El código no puede exceder 50 caracteres")]
        [Display(Name = "Código de Permiso")]
        public string CodigoPermiso { get; set; }

        [Display(Name = "Orden")]
        public int? Orden { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        // Foreign key for Opcion
        [Display(Name = "Opción")]
        public int OpcionId { get; set; }

        // Navigation property
        public virtual Opcion Opcion { get; set; }
    }
}