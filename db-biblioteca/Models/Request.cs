using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Solicitud
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        // Purchase Request (Solicitud interna) fields
        public string IdSolicitud { get; set; }
        public string AreaSolicitante { get; set; }
        public string SolicitadoPor { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Prioridad { get; set; } // alta/media/baja
        public string EstadoSolicitud { get; set; } // Pendiente / Aprobado / Rechazado / Atendido

        // Detalle fields
        public string ProductoServicio { get; set; }
        public int CantidadSolicitada { get; set; }
        public string Justificacion { get; set; }
        public DateTime FechaRequerida { get; set; }

        public string Descripcion { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}