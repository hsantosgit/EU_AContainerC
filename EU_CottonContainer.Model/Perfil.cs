using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU_CottonContainer.Model
{
    public class Perfil
    {
        public int idPerfil { get; set; }
        public string Nombre { get; set; }
        public int Status { get; set; }
        public int idAplicacion { get; set; }
        public string Nodes { get; set; }
        public string MenuPadre { get; set; }
        public int IdMenu { get; set; }   
        public bool IsChecked { get; set; }

    }
}
