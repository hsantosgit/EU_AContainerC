using EU_CottonContainer.Bussines.Facade;
using EU_CottonContainer.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EU_CottonContainer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static Usuario _user;
        private static List<Aplicacion> _lstApp;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Cuenta");
        }

        public IActionResult Index()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var data = principal.FindFirst(ClaimTypes.Name).Value;
            if (data == null)
            {
                return RedirectToAction("Salir", "Home");
            }

            _user = UsuarioFacade.GetUsuarioByUserName(data);
            _lstApp = AplicacionFacade.ObtenerAplicacionUsuario(new Aplicacion { idUsuario = _user.idUsuario }).FindAll(x => x.Status == 1);

            ViewBag.Aplicacion = _lstApp;
            ViewBag.Usuario = _user.userName;
            ViewBag.Nombre = _user.Nombre;
            ViewBag.Telefono = _user.Telefono;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
