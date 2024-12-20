using EU_CottonContainer.Data.Repository;
using EU_CottonContainer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU_CottonContainer.Bussines.Facade
{
    public static class PerfilFacade
    {
        public static List<Perfil> ObtenPerfil()
        {
            return new PerfilRepository().GetPerfil();
        }

        public static List<Perfil> ObtenPerfilBy(int iRol)
        {
            return new PerfilRepository().GetPerfilBy(iRol);
        }

        public static int DelUserOptions(Perfil entity)
        {
            return new PerfilRepository().DelUserOptions(entity);
        }

        public static int AddPerfilMenu(Perfil entity)
        {
            return new PerfilRepository().AddPerfilMenu(entity);
        }

        public static int AddPerfil(Perfil entity)
        {
            return new PerfilRepository().AddPerfil(entity);
        }

        public static int EditPerfil(Perfil entity)
        {
            return new PerfilRepository().EditPerfil(entity);
        }

    }
}
