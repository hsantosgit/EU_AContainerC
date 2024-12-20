using EU_CottonContainer.Data.Repository;
using EU_CottonContainer.Model;

namespace EU_CottonContainer.Bussines.Facade
{
    public static class AplicacionFacade
    {
        public static List<Aplicacion> ObtenerAplicacionUsuario(Aplicacion entity)
        {
            return new AplicacionRepository().GetAllBy(entity);
        }

        public static int EditApp(Aplicacion entity)
        {
            return new AplicacionRepository().EditApp(entity);
        }

        public static int AddApp(Aplicacion entity)
        {
            return new AplicacionRepository().AddApp(entity);
        }
    }   
}
