using EU_CottonContainer.Data.Repository;
using EU_CottonContainer.Model;

namespace EU_CottonContainer.Bussines.Facade
{
    public static class MenuFacade
    {
        public static int AddMenu(Menu entity)
        {
            return new MenuRepository().Add(entity);
        }

        public static int EditMenu(Menu entity)
        {
            return new MenuRepository().Edit(entity);
        }

        public static int AddSubMenu(Menu entity)
        {
            return new MenuRepository().AddSubMenu(entity);
        }

        public static int EditSubMenu(Menu entity)
        {
            return new MenuRepository().EditSubMenu(entity);
        }

        public static List<Menu> ObtenerMenuRol(Menu entity)
        {
            return new MenuRepository().GetMenuRol(entity);
        }

        public static List<Menu> ObtenerMenuPerfil(Menu entity)
        {
            return new MenuRepository().GetMenuPerfil(entity);
        }

        public static List<Menu> ObtenerMenu()
        {
            return new MenuRepository().GetMenu();
        }

    }
}
