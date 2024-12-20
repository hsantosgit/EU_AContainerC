using EU_CottonContainer.Data.Repository;
using EU_CottonContainer.Model;

namespace EU_CottonContainer.Bussines.Facade
{
    public static class SeguridadFacade
    {
        public static int SolicitudCambio(Usuario entity)
        {
            return new SeguridadRepository().SolicitudCambio(entity);
        }

        public static void UpdateExpiredPass(Usuario entity)
        {
            SeguridadRepository.UpdateExpiredPass(entity);
        }

        public static void UpdateTries(Usuario entity)
        {
            SeguridadRepository.UpdateTries(entity);
        }

        public static void UpdateToken(int idUsuario, int Bandera)
        {
            SeguridadRepository.UpdateToken(idUsuario, Bandera);
        }

        public static void UpdateTokenSMS(int idUsuario, int Bandera)
        {
            SeguridadRepository.UpdateTokenSMS(idUsuario, Bandera);
        }
    }
}
