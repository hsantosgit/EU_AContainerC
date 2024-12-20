using EU_CottonContainer.Data.Repository;
using EU_CottonContainer.Model;

namespace EU_CottonContainer.Bussines.Facade
{
    public static class UsuarioFacade
    {

        public static Usuario AutentificarUsuario(Usuario entity)
        {
            return new UsuarioRepository().GetBy(entity);
        }

        public static Usuario GetUsuarioByUserName(string name)
        {
            return new UsuarioRepository().GetByName(name);
        }

        public static List<Usuario> GetAllUsers()
        {
            return new UsuarioRepository().GetAll();
        }

        public static int AddUser(Usuario entity)
        {
            return new UsuarioRepository().Add(entity);
        }

        public static int EditUser(Usuario entity)
        {
            return new UsuarioRepository().Edit(entity);
        }

        public static int AddUserConfig(Usuario entity)
        {
            return new UsuarioRepository().AddConfigUser(entity);
        }

        public static int EditUserConfig(Usuario entity)
        {
            return new UsuarioRepository().EditConfigUser(entity);
        }

        public static int AddUserApps(Usuario entity)
        {
            return new UsuarioRepository().AddUserApps(entity);
        }

        public static int EditUserApps(Usuario entity)
        {
            return new UsuarioRepository().EditUserApps(entity);
        }

        public static int UpdateUser(Usuario entity)
        {
            return new UsuarioRepository().Update(entity);
        }

        //public static int ActualizaSesion(Usuario entity)
        //{
        //    return new UsuarioRepository().UpdateSesion(entity);
        //}

        public static int ActualizaStatus(Usuario entity)
        {
            return new UsuarioRepository().UpdateStatus(entity);
        }

        public static int SaveToken(Usuario entity)
        {
            return new UsuarioRepository().SaveToken(entity);
        }
    }
}
