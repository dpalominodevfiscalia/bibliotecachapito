using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Correo { get; set; }
        public string Estado { get; set; } = "Activo";
        public int IdRol { get; set; }
        public virtual Rol Rol { get; set; }
        public int IdPerfil { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}