using EU_CottonContainer.Bussines.Facade;
using EU_CottonContainer.Model;
using Microsoft.AspNetCore.Mvc;

namespace EU_CottonContainer.ViewComponents
{
    public class AplicacionViewComponent : ViewComponent
    {
        private static Usuario _user;
        public AplicacionViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Aplicacion> _apps = ControlFacade.GetAllAplicaciones();
            return View(_apps);
        }
    }
}
