using System.ComponentModel.DataAnnotations;

namespace EU_CottonContainer.Model
{
    [Serializable]
    public class Menu
    {
        public int idMenu { get; set; }
        public int idAplicacion { get; set; }
        public int isTitle { get; set; }
        public int idPadre { get; set; }
        public string Icon { get; set; }
        public string Nombre { get; set; }
        public string Url { get; set; }
        public int Orden { get; set; }
        public int Status { get; set; }
        public int idRol { get; set; }
        public int idPerfil { get; set; }
        public string SubMenu { get; set; }
        
    }
}
