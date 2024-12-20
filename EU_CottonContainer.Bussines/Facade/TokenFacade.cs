using EU_CottonContainer.Data.Repository;
using EU_CottonContainer.Model;

namespace EU_CottonContainer.Bussines.Facade
{
    public static class TokenFacade
    {
        public static List<Token> ObtenTokenList()
        {
            return new TokenRepository().GetTokenList();
        }

        public static int DelUserToken(int idUsuario)
        {
            return new TokenRepository().DelUserToken(idUsuario);
        }
    }
}
