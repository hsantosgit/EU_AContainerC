using EU_CottonContainer.Data.Repository;
using EU_CottonContainer.Model;

namespace EU_CottonContainer.Bussines.Facade
{
    public static class RolFacade
    {
        public static List<Rol> ObtenRol()
        {
            return new RolRepository().GetRol();
        }

        public static int AddRol(Rol entity)
        {
            return new RolRepository().AddRol(entity);
        }

        public static int DelRolOptions(Rol entity)
        {
            return new RolRepository().DelRolOptions(entity);
        }

        public static int AddRolPerfil(Rol entity)
        {
            return new RolRepository().AddRolPerfil(entity);
        }

        public static int EditRol(Rol entity)
        {
            return new RolRepository().EditRol(entity);
        }
    }
}
