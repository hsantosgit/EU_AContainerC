using EU_CottonContainer.Bussines.Facade;
using EU_CottonContainer.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EU_CottonContainer.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private static Usuario _user;
        public MenuViewComponent()
        {
            //ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            //var data = principal.FindFirst(ClaimTypes.Name);

            //ClaimsPrincipal claimuser = HttpContext.User;
            //string nombreUsuario = "";

            //if (claimuser.Identity.IsAuthenticated)
            //{
            //    nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            //}
            //_user = UsuarioFacade.GetUsuarioByUserName(nombreUsuario);
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var nombreUsuario = principal.FindFirst(ClaimTypes.Name);
            _user = UsuarioFacade.GetUsuarioByUserName(nombreUsuario.Value);
            Menu _menu = new Menu { idRol = _user.idRol };
            List<Menu> _lstMenu = MenuFacade.ObtenerMenuRol(_menu);
            return View(_lstMenu);
        }


    }
}
