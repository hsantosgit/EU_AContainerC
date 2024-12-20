using EU_CottonContainer.Data.Repository;
using EU_CottonContainer.Model;

namespace EU_CottonContainer.Bussines.Facade
{
    public static class ControlFacade
    {
        public static List<Rol> GetAllRoles()
        {
            return new ControlRepository().GetRolList();
        }

        public static List<Planta> GetAllPlantas()
        {
            return new ControlRepository().GetPlantaList();
        }

        public static List<Aplicacion> GetAllAplicaciones()
        {
            return new ControlRepository().GetAplicacionList();
        }

        public static List<Usuario> GetStatusUsuario()
        {
            return new ControlRepository().GetStatusUserList();
        }

        public static List<Aplicacion> GetAllAplicacionesBy(string _sUser)
        {
            return new ControlRepository().GetAplicacionListBy(_sUser);
        }
    }
}
