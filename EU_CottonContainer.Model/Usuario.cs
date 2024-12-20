using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EU_CottonContainer.Model
{
    [Serializable]
    public class Usuario
    {
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "Debe capturar un nombre de Usuario")]
        [Display(Name = "Usuario")]
        public string? userName { get; set; }

        [Required(ErrorMessage = "Debe capturar el nombre del Usuario")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "Debe capturar el Apellido Paterno del Usuario")]
        [Display(Name = "A. Paterno")]
        public string? apPaterno { get; set; }

        [Display(Name = "A. Materno")]
        public string? apMaterno { get; set; }

        [Required(ErrorMessage = "Debe capturar el Teléfono del Usuario")]
        [Display(Name = "Telefono")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "Debe capturar el Correo del Usuario")]
        [Display(Name = "Correo")]
        public string? Correo { get; set; }

        public string? CorreoAlterno { get; set; }

        public string? Sesion { get; set; }

        public int Status { get; set; }
        public string? Abreviatura { get; set; }


        public string? FechaAlta { get; set; }

        [Required(ErrorMessage = "Debe capturar la Contraseña del Usuario")]
        [Display(Name = "Contraseña")]
        public string? Password  { get; set; }

        public string? ConfirmPassword { get; set; }

        public int idRol { get; set; }
        public string? Rol { get; set; }

        public int idPlanta { get; set; }
        public string? Planta { get; set; }

        public int idAplicacion { get; set; }
        //public int SMSCode { get; set; }
        //tbl Usuario:Config
        public string? FechaPass { get; set; }
        public string? FechaBLQ { get; set; }
        public int IsToken { get; set; }
        public bool boolToken { get; set; }
        public int IsTokenSMS { get; set; }
        public bool boolTokenSMS { get; set; }
        public int IsTokenMail { get; set; }
        public bool boolTokenMail { get; set; }
        public int Intentos { get; set; }

        public string? Token { get; set; }
        public string? TokenValidate { get; set; }
        public int IsTokenActive { get; set; }
        public string FechaToken { get; set; }

        [NotMapped]
        public bool MantenerActivo { get; set; }

        public int[] AplicacionIds { get; set; }

        public List<string> AppIds { get; set; }

        public bool boolName { get; set; }
        public bool boolUserName { get; set; }
        public bool boolEmail { get; set; }
        public bool boolEmailAlterno { get; set; }
        public bool boolPhone { get; set; }

    }
}
