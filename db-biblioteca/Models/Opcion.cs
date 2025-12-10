using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Opcion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la opción es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(255, ErrorMessage = "La descripción no puede exceder 255 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El controlador es obligatorio")]
        [StringLength(50, ErrorMessage = "El controlador no puede exceder 50 caracteres")]
        [Display(Name = "Controlador")]
        public string Controlador { get; set; }

        [Required(ErrorMessage = "La acción es obligatoria")]
        [StringLength(50, ErrorMessage = "La acción no puede exceder 50 caracteres")]
        [Display(Name = "Acción")]
        public string Accion { get; set; }

        [StringLength(50, ErrorMessage = "El icono no puede exceder 50 caracteres")]
        [Display(Name = "Icono")]
        public string Icono { get; set; }

        [Display(Name = "Orden")]
        public int? Orden { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        [Display(Name = "Es Menu Principal")]
        public bool EsMenuPrincipal { get; set; } = false;

        [Display(Name = "Requiere Autenticación")]
        public bool RequiereAutenticacion { get; set; } = true;

        // Foreign key for Modulo
        [Display(Name = "Módulo")]
        public int ModuloId { get; set; }

        // Navigation property
        public virtual Modulo Modulo { get; set; }

        // Navigation property for actions/permissions
        public virtual ICollection<OpcionAccion> OpcionAcciones { get; set; }

        public Opcion()
        {
            OpcionAcciones = new HashSet<OpcionAccion>();
        }
    }
}