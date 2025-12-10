using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaDB.Models
{
    public class Catalogo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CatalogoId { get; set; }

        [Required]
        [StringLength(100)]
        public string GrupoName { get; set; }

        [Required]
        [StringLength(100)]
        public string GrupoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Valor { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "Activo";

        [StringLength(100)]
        public string Usuario { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}