using EU_CottonContainer.Bussines.Facade;
using EU_CottonContainer.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EU_CottonContainer.ViewComponents
{
    public class UserViewComponent : ViewComponent
    {
        private static Usuario _user;

        public UserViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal principal = HttpContext.User;
            var nombreUsuario = principal.FindFirst(ClaimTypes.Name);
            _user = UsuarioFacade.GetUsuarioByUserName(nombreUsuario.Value);
            return View(_user);
        }
    }
}
