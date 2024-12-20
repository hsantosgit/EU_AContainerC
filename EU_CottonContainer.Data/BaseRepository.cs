using EU_CottonContainer.ProviderData;
using EU_CottonContainer.Security;

namespace EU_CottonContainer.Data
{
    public class BaseRepository
    {
        protected DataConsumer _ProviderDB;

        protected BaseRepository()
        {
            _ProviderDB = DataFactory.GetNewInstance(
                                    GlobalConfiguration.StringConectionDB,
                                    GlobalConfiguration.ProviderDB);

            _ProviderDB.AutoOpenAndCloseConnectionForDataReader = true;
        }
    }
}
