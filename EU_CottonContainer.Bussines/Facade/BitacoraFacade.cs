using EU_CottonContainer.Data.Repository;
using EU_CottonContainer.Model;

namespace EU_CottonContainer.Bussines.Facade
{
    public static class BitacoraFacade
    {
        public static void AddBitacora(Bitacora entity)
        {
            BitacoraRepository.Add(entity);
        }
    }
}
