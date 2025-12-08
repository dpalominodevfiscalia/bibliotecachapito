using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class TipoCliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; } = "Activo";

        // Navigation property
        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}