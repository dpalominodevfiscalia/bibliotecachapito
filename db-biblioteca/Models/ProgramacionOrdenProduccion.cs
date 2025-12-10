using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaDB.Models
{
    public class ProgramacionOrdenProduccion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Código")]
        public string CodigoOrden { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(500)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Fecha Programada")]
        public DateTime FechaProgramada { get; set; }

        [Required]
        [Display(Name = "Fecha Inicio")]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        public DateTime? FechaFin { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [StringLength(50)]
        [Display(Name = "Prioridad")]
        public string Prioridad { get; set; }

        [StringLength(100)]
        [Display(Name = "Responsable")]
        public string Responsable { get; set; }

        [StringLength(100)]
        [Display(Name = "Creado Por")]
        public string CreadoPor { get; set; }

        [Display(Name = "Fecha Creación")]
        public DateTime FechaCreacion { get; set; }

        [StringLength(100)]
        [Display(Name = "Modificado Por")]
        public string ModificadoPor { get; set; }

        [Display(Name = "Fecha Modificación")]
        public DateTime? FechaModificacion { get; set; }
    }
}