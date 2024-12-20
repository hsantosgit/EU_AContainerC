using EU_CottonContainer.Bussines.Facade;
using EU_CottonContainer.Helpers;
using EU_CottonContainer.Model;
using EU_CottonSecurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EU_CottonContainer.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private static Usuario _user;
        // GET: MenuController
        public ActionResult Index()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var data = principal.FindFirst(ClaimTypes.Name).Value;
            if (data == null)
            {
                return RedirectToAction("Salir", "Home");
            }
            _user = UsuarioFacade.GetUsuarioByUserName(data);
            if (!ValidaAccesoUsuario())
            {
                return RedirectToAction("Index", "Home");
            }
            List<Menu> _lstmenu = new List<Menu>();
            _lstmenu = MenuFacade.ObtenerMenu();
            return View(_lstmenu);
        }

        public ActionResult AddMenu()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var data = principal.FindFirst(ClaimTypes.Name).Value;
            if (data == null)
            {
                return RedirectToAction("Salir", "Home");
            }
            _user = UsuarioFacade.GetUsuarioByUserName(data);
            if (!ValidaAccesoUsuario())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public string SaveMenu(Menu menu)
        {
            string resp = string.Empty;
            try
            {
                if (validaCamposBase(menu))
                {
                    resp = _user.Nombre;
                }
                else
                {
                    menu.idAplicacion = 0;
                    if (MenuFacade.AddMenu(menu) > 0)
                    {
                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 5, Accion = "Se agregó menú : " + menu.Nombre, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                        resp = "OK";
                    }
                    else
                    {
                        resp = GlobalConfiguration.ExceptionError;
                    }
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en alta de menú \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }

            return resp;
        }

        [HttpPost]
        public ActionResult AddSubMenu(int _idMenu)
        {
            List<Menu> _lstmenu = MenuFacade.ObtenerMenu();

            Menu _men = new Menu();
            foreach (Menu menu in _lstmenu.FindAll(x => x.idMenu == _idMenu))
            {
                _men.SubMenu = menu.Nombre.ToString();
            }

            _lstmenu = _lstmenu.FindAll(x => x.idPadre == _idMenu);
            ViewBag.Opciones = _lstmenu;
            _men.Orden = _lstmenu.Count + 1;
            _men.idPadre = _idMenu;

            return View(_men);
        }

        [HttpPost]
        public ActionResult EditMenu(int _idMenu)
        {
            List<Menu> _lstmenu = MenuFacade.ObtenerMenu();

            Menu _men = new Menu();
            foreach (Menu menu in _lstmenu.FindAll(x => x.idMenu == _idMenu))
            {
                _men.idMenu = menu.idMenu;
                _men.Nombre = menu.Nombre.ToString();
                _men.Icon = menu.Icon;
                _men.Orden = menu.Orden;
                _men.Status = menu.Status;  
                _men.Url = "#" + menu.Nombre;    
            }

            return View(_men);  
        }

        [HttpPost]
        public string SaveMenuEdit(Menu menu)
        {
            string resp = string.Empty;
            try
            {
                if (validaCamposBase(menu))
                {
                    resp = _user.Nombre;
                }
                else
                {
                    menu.idAplicacion = 0;
                    if (MenuFacade.EditMenu(menu) > 0)
                    {
                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 5, Accion = "Se editó menú : " + menu.Nombre, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                        resp = "OK";
                    }
                    else
                    {
                        resp = GlobalConfiguration.ExceptionError;
                    }
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en alta de menú \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }

            return resp;
        }

        [HttpPost]
        public ActionResult EditSubMenu(int _idMenu)
        {
            List<Menu> _lstmenu = MenuFacade.ObtenerMenu();

            Menu _men = new Menu();
            foreach (Menu menu in _lstmenu.FindAll(x => x.idMenu == _idMenu))
            {
                _men = menu;
            }

            List<Menu> _lstOpciones = _lstmenu.FindAll(x => x.idPadre == _men.idPadre);
            List<Menu> _lsPadre = _lstmenu.FindAll(x => x.idMenu == _men.idPadre);
            ViewBag.Padre = _lsPadre[0].Nombre;
            ViewBag.MenuList = _lstOpciones;
            return View(_men);
        }


        [HttpPost]
        public string SaveSubMenu(Menu menu)
        {
            string resp = string.Empty;
            try
            {
                if (validaCamposBaseEdit(menu))
                {
                    resp = _user.Nombre;
                }
                else
                {
                    menu.idAplicacion = 0;
                    if (MenuFacade.AddSubMenu(menu) > 0)
                    {
                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 5, Accion = "Se agregó submenú : " + menu.Nombre, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                        resp = "OK";
                    }
                    else
                    {
                        resp = GlobalConfiguration.ExceptionError;
                    }
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en alta de menú \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }

            return resp;
        }

        [HttpPost]
        public string SaveSubMenuEdit(Menu menu)
        {
            string resp = string.Empty;
            try
            {
                if (validaCamposBaseEdit(menu))
                {
                    resp = _user.Nombre;
                }
                else
                {
                    menu.idAplicacion = 0;
                    if (MenuFacade.EditSubMenu(menu) > 0)
                    {
                        BitacoraFacade.AddBitacora(new Bitacora { idUsuario = _user.idUsuario, idMenu = 5, Accion = "Se edito submenú : " + menu.Nombre, ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString(), mcAddress = location.GetMACAddress(), Ubicacion = location.GetGeoCodedResults(this.HttpContext.Connection.RemoteIpAddress.ToString()).Status.ToString() });
                        resp = "OK";
                    }
                    else
                    {
                        resp = GlobalConfiguration.ExceptionError;
                    }
                }
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Error en alta de menú \n" + "Error: " + ex.Message);
                resp = GlobalConfiguration.ExceptionError;
            }

            return resp;
        }


        protected bool validaCamposBase(Menu menu)
        {
            bool valida = false;
            if (menu.Icon == string.Empty || menu.Icon == null)
            {
                _user.Nombre = "Seleccione uno de los iconos mostrados.";
                valida = true;
            }
            else if (menu.Nombre == string.Empty)
            {
                _user.Nombre = "Capture nombre de menú.";
                valida = true;
            }

            return valida;
        }

        protected bool validaCamposBaseEdit(Menu menu)
        {
            bool valida = false;
            if (menu.Url == string.Empty || menu.Url == null)
            {
                _user.Nombre = "Capture url de la opción de menú.";
                valida = true;
            }
            else if (menu.Nombre == string.Empty)
            {
                _user.Nombre = "Capture nombre de menú.";
                valida = true;
            }
            else if (menu.Orden.ToString() == string.Empty)
            {
                _user.Nombre = "Capture número de orden de opción de menú.";
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
                //5->opción Menú
                if (m.idMenu == 5)
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
