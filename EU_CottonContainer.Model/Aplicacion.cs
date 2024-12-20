using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EU_CottonContainer.Model
{
    [Serializable]
    public class Aplicacion
    {
        public int idAplicacion { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Url")]
        public string Url { get; set; }

        public string icono { get; set; }

        public int Status { get; set; }

        public int idUsuario { get; set; }

        [NotMapped]
        public bool isChecked { get; set; } 

        public string? Mensaje { get; set; }
    }
}
