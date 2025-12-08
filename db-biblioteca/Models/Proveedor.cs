using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaDB.Models
{
    public class Proveedor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La razón social es obligatoria")]
        [StringLength(200, ErrorMessage = "La razón social no puede exceder 200 caracteres")]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "El RUC es obligatorio")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "El RUC debe tener exactamente 11 caracteres")]
        public string RUC { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        public string Direccion { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "El contacto no puede exceder 100 caracteres")]
        public string Contacto { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono de contacto no puede exceder 20 caracteres")]
        public string TelefonoContacto { get; set; }

        [EmailAddress(ErrorMessage = "El email de contacto no tiene un formato válido")]
        [StringLength(100, ErrorMessage = "El email de contacto no puede exceder 100 caracteres")]
        public string EmailContacto { get; set; }

        [Required(ErrorMessage = "El tipo de proveedor es obligatorio")]
        public string TipoProveedor { get; set; }

        [StringLength(100, ErrorMessage = "Las condiciones de pago no pueden exceder 100 caracteres")]
        public string CondicionesPago { get; set; }

        [StringLength(100, ErrorMessage = "El banco no puede exceder 100 caracteres")]
        public string Banco { get; set; }

        [StringLength(50, ErrorMessage = "El número de cuenta no puede exceder 50 caracteres")]
        public string NumeroCuenta { get; set; }

        [Required(ErrorMessage = "La moneda es obligatoria")]
        public string Moneda { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string Observaciones { get; set; }

        public string Estado { get; set; } = "Activo";
    }
}