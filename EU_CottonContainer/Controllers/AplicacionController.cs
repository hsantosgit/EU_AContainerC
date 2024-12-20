using EU_CottonContainer.Bussines.Facade;
using EU_CottonContainer.Helpers;
using EU_CottonContainer.Model;
using EU_CottonSecurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EU_CottonContainer.Controllers
{
    [Authorize]
    public class AplicacionController : Controller
    {
        // GET: AplicacionController
        private static Usuario _user;
        private static List<Aplicacion> _lstApp = new List<Aplicacion>();
        public ActionResult Index()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var data = principal.FindFirst(ClaimTypes.Name).Value;
            if (data == null)
            {
                return RedirectToAction("Salir", "Home");
            }
            _user = UsuarioFacade.GetUsuarioByUserName(data);

            //Validar que el usuario tenga accesso a la página
            if (!ValidaAccesoUsuario())
            {
                return RedirectToAction("Index", "Home");
            }
            _lstApp = ControlFacade.GetAllAplicaciones();
            return View(_lstApp);
        }

        public ActionResult AddApp()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var data = principal.FindFirst(ClaimTypes.Name).Value;
            if (data == null)
            {
                return RedirectToAction("Salir", "Home");
            }
            _user = UsuarioFacade.GetUsuarioByUserName(data);

            //Validar que el usuario tenga accesso a la página
            if (!ValidaAccesoUsuario())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public string SaveAddApp(Aplicacion app)
        {
            string resp = string.Empty;
            try
            {
                if (validaCamposEdit(app))
                {
                    resp = app.Mensaje;
                }
                else
                {
                    AplicacionFacade.AddApp(app);
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = 1, idMenu = 3, Accion = "alta de aplicación: " + app.Nombre, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                    resp = "OK";
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en alta de aplicación \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        [HttpPost]
        public ActionResult EditApp(int _idApp)
        {
            _lstApp = ControlFacade.GetAllAplicaciones();
            Aplicacion _app = new Aplicacion();
            foreach (var app in _lstApp.FindAll(x => x.idAplicacion == _idApp))
            {
                _app = new Aplicacion()
                {
                    idAplicacion = app.idAplicacion,
                    Nombre = app.Nombre,
                    icono = app.icono,
                    Url = app.Url,
                    Status = app.Status
                };
            }

            return View(_app);
        }

        [HttpPost]
        public string SaveEditApp(Aplicacion app)
        {
            string resp = string.Empty;
            try
            {
                if (validaCamposEdit(app))
                {
                    resp = app.Mensaje;
                }
                else
                {
                    AplicacionFacade.EditApp(app);
                    BitacoraFacade.AddBitacora(new Bitacora { idUsuario = 1, idMenu = 3, Accion = "Actualización de aplicación: " + app.Nombre, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                    resp = "OK";
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en actualización de usuario \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }
            return resp;
        }

        protected bool validaCamposEdit(Aplicacion app)
        {
            bool valida = false;
            if (string.IsNullOrEmpty(app.Url))
            {
                app.Mensaje = "Capture url de aplicación";
                valida = true;
            }
            else if (string.IsNullOrEmpty(app.Nombre))
            {
                _user.Nombre = "Capture nombre de Aplicación.";
                valida = true;
            }
            else if (string.IsNullOrEmpty(app.icono))
            {
                _user.Nombre = "Capture Icono de aplicación";
                valida = true;
            }

            return valida;
        }

        private bool ValidaAccesoUsuario()
        {
            bool valida = false;
            Menu _menu = new Menu { idRol = _user.idRol };
            List<Menu> _lstMenu = MenuFacade.ObtenerMenuRol(_menu);
            int busca = 0;
            foreach (var m in _lstMenu)
            {
                //8 -> opción Aplicaciones
                if (m.idMenu == 8)
                    busca = m.idMenu;
            }

            if (busca > 0)
            {
                valida = true;
            }

            return valida;
        }
    }
}
